using System;
using System.Linq;
using Enemies.Armadillo.Animation;
using Enemies.Armadillo.Behaviour;
using Enemies.Common.Behaviour;
using Omnia.State;
using Players;
using UnityEngine;

namespace Enemies.Armadillo {
    public class Armadillo : Enemy {
        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask player;
        [SerializeField] internal BoxCollider2D[] checks;
        [SerializeField] internal Vector2 facing = Vector2.right;
        [SerializeField] internal float hitDistance = 1;
        [SerializeField] internal float walkSpeed = 1f;
        [SerializeField] internal float rollSpeed = 4f;
        [SerializeField] private string debugBehaviour;

        public void Awake() {
            UseBehaviour(new Walk(this));
        }

        override public void Update() {
            base.Update();
            sprite.flipX = facing.x == 0 ? sprite.flipX : facing.x > 0;
        }

        override public void FixedUpdate() {
            base.FixedUpdate();
        }

        public void Attack(GameObject it) {
            Player targetPlayer = it.GetComponent<Player>();
            if (targetPlayer != null) {
                float radians = knockbackAngle * Mathf.Deg2Rad;
                Vector2 knockback = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * knockbackForce;

                knockback.x *= Mathf.Sign(facing.x);

                targetPlayer.Hurt(attack, knockback, 1);
            }
        }

        override public void UseBehaviour(IBehaviour behaviour) {
            base.UseBehaviour(behaviour);
            debugBehaviour = behaviour.GetType().Name;
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            var idle = new IdleAnimation(animator);
            var walk = new WalkAnimation(animator);
            var alert = new AlertAnimation(animator);
            var rush = new RushAnimation(animator);
            var stun = new StunAnimation(animator);
            var uncurl = new UncurlAnimation(animator);

            stateMachine.AddAnyTransition(idle, new FuncPredicate(() => behaviour is Walk && rb.velocity.x == 0)); // Stay standing up
            stateMachine.AddAnyTransition(walk, new FuncPredicate(() => behaviour is Walk && rb.velocity.x != 0));
            stateMachine.AddAnyTransition(alert, new FuncPredicate(() => behaviour is Alert));
            stateMachine.AddAnyTransition(rush, new FuncPredicate(() => behaviour is Rush && rb.velocity.x != 0));
            stateMachine.AddAnyTransition(stun, new FuncPredicate(() => behaviour is Stun));
            stateMachine.AddAnyTransition(uncurl, new FuncPredicate(() => behaviour is Uncurl));

            stateMachine.SetState(walk);
            animationStateMachine = stateMachine;
        }
    }
}
