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
            UseBehaviour(new Idle(this));

            /* TODO: perf cost? */
            targetInstance ??= FindObjectsOfType<Player>().FirstOrDefault();
        }

        public void OnExplode() {
            // Instantiate(explosion, transform.position, transform.rotation);
            Attack();
            Hurt(maximumHealth);
        }

        public bool IsTargetDetected(out Player it) {
            it = targetInstance;
            return it && Sweep(transform.position, it.sprite.transform.position - sprite.transform.position, 45, detectionRadius, 5, ground | player).Any(hit => IsOnLayer(hit, player));
        }

        private void Attack() {
            var hit = Physics2D.OverlapCircle(transform.position, explosionRadius, player);
            if (!hit || !hit.TryGetComponent<Player>(out var it)) return;
            var direction = it.sprite.transform.position - transform.position;

            it.Hurt(attack, direction.normalized * knockbackForce, 5);
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            return;
            var idleAnim = new IdleAnimation(animator);
            var alertAnim = new AlertAnimation(animator);
            var attackAnim = new AttackAnimation(animator);

            stateMachine.AddAnyTransition(idleAnim, new FuncPredicate(() => behaviour is Idle));
            stateMachine.AddAnyTransition(alertAnim, new FuncPredicate(() => behaviour is Alert));
            stateMachine.AddAnyTransition(attackAnim, new FuncPredicate(() => behaviour is Attack));

            stateMachine.SetState(idleAnim);
            animationStateMachine = stateMachine;
        }
    }
}
