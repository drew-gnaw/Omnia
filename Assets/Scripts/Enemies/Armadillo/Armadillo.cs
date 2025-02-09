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

        protected override void UseAnimation(StateMachine stateMachine) {
            var idle = new IdleAnimation(animator);
            var walk = new WalkAnimation(animator);
            var rush = new RushAnimation(animator);
            var stun = new StunAnimation(animator);
            var stagger = new StaggerAnimation(animator);

            stateMachine.AddAnyTransition(idle, new FuncPredicate(() => behaviour is Walk && rb.velocity.x == 0 || behaviour is Rush && rb.velocity.x == 0));
            stateMachine.AddAnyTransition(walk, new FuncPredicate(() => behaviour is Walk && rb.velocity.x != 0));
            stateMachine.AddAnyTransition(rush, new FuncPredicate(() => behaviour is Rush && rb.velocity.x != 0));
            stateMachine.AddAnyTransition(stun, new FuncPredicate(() => behaviour is Stun));
            stateMachine.AddAnyTransition(stagger, new FuncPredicate(() => behaviour is Stagger));

            stateMachine.SetState(idle);
            animationStateMachine = stateMachine;
        }
    }
}
