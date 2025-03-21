using System;
using System.Linq;
using Enemies.Bird.Animation;
using Enemies.Bird.Behaviour;
using Omnia.State;
using Players;
using UnityEngine;
using StateMachine = Omnia.State.StateMachine;

namespace Enemies.Bird {
    public class Bird : Enemy {
        [SerializeField] internal float detectionRadius;
        [SerializeField] internal float fuse;
        [SerializeField] internal float triggerDistance;
        [SerializeField] internal float explosionRadius;
        [SerializeField] internal float delay;
        [SerializeField] internal float speed;
        [SerializeField] internal float airAcceleration;

        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal GameObject explosion;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask player;
        [SerializeField] internal Player targetInstance;

        public Action<Bird> NotifyOnDestroy;

        public void OnDestroy() => NotifyOnDestroy?.Invoke(this);

        public void Awake() {
            targetInstance ??= FindObjectsOfType<Player>().FirstOrDefault();
            UseBehaviour(new Idle(this));
        }

        public override void Update() {
            base.Update();
            sprite.flipX = rb.velocity.x == 0 ? sprite.flipX : rb.velocity.x > 0;
        }

        public void OnExplode() {
            Instantiate(explosion, transform.position, transform.rotation);
            Attack();
            Hurt(maximumHealth);
        }

        public bool IsTargetDetected() =>
            targetInstance &&
            Sweep(transform.position, targetInstance.sprite.transform.position - sprite.transform.position, 45, detectionRadius, 5, ground | player).Any(hit => IsOnLayer(hit, player));

        private void Attack() {
            var hit = Physics2D.OverlapCircle(transform.position, explosionRadius, player);
            if (!hit || !hit.TryGetComponent<Player>(out var it)) return;
            var direction = it.sprite.transform.position - transform.position;

            it.Hurt(attack, direction.normalized * knockbackForce, 5);
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            var idleAnim = new IdleAnimation(animator);
            stateMachine.AddAnyTransition(idleAnim, new FuncPredicate(() => behaviour is Idle));
            stateMachine.AddAnyTransition(new AlertAnimation(animator), new FuncPredicate(() => behaviour is Alert));
            stateMachine.AddAnyTransition(new AttackAnimation(animator), new FuncPredicate(() => behaviour is Attack));

            stateMachine.SetState(idleAnim);
            animationStateMachine = stateMachine;
        }
    }
}
