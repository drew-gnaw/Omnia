using System;
using System.Linq;
using Enemies.Armadillo.Animation;
using Enemies.Armadillo.Behaviour;
using Omnia.State;
using UnityEngine;

namespace Enemies.Armadillo {
    public class Armadillo : EnemyBase {
        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask player;
        [SerializeField] internal BoxCollider2D[] checks;
        [SerializeField] internal Vector2 facing = Vector2.right;

        private IBehaviour behaviour;
        private StateMachine animationStateMachine;

        public void Awake() {
            UseBehaviour(new Walk(this));
            UseAnimation(new StateMachine());
        }

        public void Update() {
            sprite.flipX = facing.x == 0 ? sprite.flipX : Math.Sign(facing.x) == 1;

            animationStateMachine?.Update();
        }

        public void FixedUpdate() {
            behaviour?.OnTick();
            animationStateMachine?.FixedUpdate();
        }

        public void UseBehaviour(IBehaviour it) {
            behaviour?.OnExit();
            behaviour = it;
            behaviour?.OnEnter();
        }

        public void OnAttack(GameObject it) {
            /* TODO: Not implemented yet. Take self-damage for testing purposes. */
            Debug.Log(this + " attacked " + it);
            TakeDamage(4);
        }

        private void UseAnimation(StateMachine stateMachine) {
            var idle = new IdleAnimation(animator);
            var walk = new WalkAnimation(animator);
            var rush = new RushAnimation(animator);
            var stun = new StunAnimation(animator);

            stateMachine.AddAnyTransition(idle, new FuncPredicate(() => behaviour is Walk && rb.velocity.x == 0 || behaviour is Rush && rb.velocity.x == 0));
            stateMachine.AddAnyTransition(walk, new FuncPredicate(() => behaviour is Walk && rb.velocity.x != 0));
            stateMachine.AddAnyTransition(rush, new FuncPredicate(() => behaviour is Rush && rb.velocity.x != 0));
            stateMachine.AddAnyTransition(stun, new FuncPredicate(() => behaviour is Stun));

            stateMachine.SetState(idle);
            animationStateMachine = stateMachine;
        }

        /* TODO: This is a common utility function and should be moved. */
        public static bool IsOnLayer(RaycastHit2D hit, LayerMask mask) {
            return hit && (mask & 1 << hit.collider.gameObject.layer) != 0;
        }

        /* TODO: This is a common utility function and should be moved. */
        public static RaycastHit2D[] Sweep(Vector2 origin, Vector2 direction, float angle, float distance, int count, LayerMask mask) {
            var step = count == 1 ? 0 : angle / (count - 1);
            var initial = angle / 2f;
            return Enumerable.Range(0, count).Select(it => Raycast(origin, direction, initial - step * it, distance, mask)).ToArray();
        }

        private static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float angle, float distance, LayerMask mask) {
            return Physics2D.Raycast(origin, Quaternion.Euler(0, 0, angle) * direction.normalized, distance, mask);
        }
    }
}
