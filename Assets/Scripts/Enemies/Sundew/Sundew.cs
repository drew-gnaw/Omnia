using System.Linq;
using Enemies.Sundew.Behaviour;
using Enemies.Sundew.Animation;
using Enemies.Common.Behaviour;
using Omnia.State;
using Players;
using UnityEngine;
using Utils;

namespace Enemies.Sundew {
    public class Sundew : Enemy {
        [SerializeField] internal float distance;
        [SerializeField] internal float windup;
        [SerializeField] internal float reload;
        [SerializeField] internal float spread;
        [SerializeField] internal float arc;
        [SerializeField] internal float count;

        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask player;
        [SerializeField] internal LayerMask bg;
        [SerializeField] internal GameObject projectile;

        [SerializeField] internal bool detected;

        [SerializeField] internal string debugBehaviour;

        private float t;

        public void Awake() {
            UseBehaviour(Idle.AsDefaultOf(this));
        }

        override public void Update() {
            base.Update();
            t = Mathf.Max(0, t - Time.deltaTime);
        }

        override public void FixedUpdate() {
            base.FixedUpdate();
            UseDetection();
        }

        public void FireProjectiles() {
            var g = Mathf.Abs(Physics2D.gravity.y);
            var y = Mathf.Sqrt(arc * g * 2);

            for (var i = 0; i < count; i++) {
                var d = distance - i * spread;
                var x = d * g / (y * 2);

                FireProjectile(new Vector2(x, y));
                FireProjectile(new Vector2(x * -1, y));
            }
        }

        override public void UseBehaviour(IBehaviour it) {
            if (it == null) return;
            debugBehaviour = it.GetType().Name;

            base.UseBehaviour(it);
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            var idle = new IdleAnimation(animator);
            var attack = new AttackAnimation(animator);
            var hide = new HideAnimation(animator);
            var reveal = new RevealAnimation(animator);
            var windUp = new WindUpAnimation(animator);
            var stagger = new StaggerAnimation(animator);

            stateMachine.AddAnyTransition(idle, new FuncPredicate(() => behaviour is Idle));
            stateMachine.AddAnyTransition(attack, new FuncPredicate(() => behaviour is Attack));
            stateMachine.AddAnyTransition(hide, new FuncPredicate(() => behaviour is Hide));
            stateMachine.AddAnyTransition(reveal, new FuncPredicate(() => behaviour is Reveal));
            stateMachine.AddAnyTransition(windUp, new FuncPredicate(() => behaviour is WindUp));
            stateMachine.AddAnyTransition(stagger, new FuncPredicate(() => behaviour is Stagger));

            stateMachine.SetState(idle);
            animationStateMachine = stateMachine;
        }

        public void SetLayer(LayerMask it) {
            gameObject.layer = MathUtils.LayerIndexOf(it);
        }

        private void UseDetection() {
            if (t != 0) return;
            t = 0.25f;
            detected = Sweep(sprite.transform.position, Vector2.up, 180, distance, 10, ground | player).Any(it => IsOnLayer(it, player));
        }

        private void Attack(Player it) {
            float direction = Mathf.Sign(it.transform.position.x - transform.position.x);

            float radians = knockbackAngle * Mathf.Deg2Rad;
            Vector2 knockback = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * knockbackForce;

            knockback.x *= direction;

            it.Hurt(attack, knockback, 1);
        }

        private void FireProjectile(Vector2 velocity) {
            var instance = Instantiate(projectile, sprite.transform.position, sprite.transform.rotation).GetComponent<SundewProjectile>().Of(Attack);
            var noise = new Vector2(Random.Range(0.25f * -1, 0.25f), Random.Range(0.25f * -1, 0.25f));

            instance.rb.velocity = velocity + noise;
            Destroy(instance.gameObject, windup + reload);
        }
    }
}
