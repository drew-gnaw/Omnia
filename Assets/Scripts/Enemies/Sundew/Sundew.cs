using System.Linq;
using Enemies.Sundew.Behaviour;
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
        [SerializeField] internal float damage;

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
        private IBehaviour behaviour;

        public void Awake() {
            UseBehaviour(Idle.AsDefaultOf(this));
        }

        public void Update() {
            t = Mathf.Max(0, t - Time.deltaTime);
            behaviour?.OnUpdate();
        }

        public void FixedUpdate() {
            UseDetection();
            behaviour?.OnTick();
        }

        public void FireProjectiles() {
            var g = Mathf.Abs(Physics2D.gravity.y);
            var y = Mathf.Sqrt(arc * g * 2);

            for (var i = 0; i < 3; i++) {
                var d = distance - i * spread;
                var x = d * g / (y * 2);

                FireProjectile(new Vector2(x, y));
                FireProjectile(new Vector2(x * -1, y));
            }
        }

        public void UseBehaviour(IBehaviour it) {
            if (it == null) return;
            debugBehaviour = it.GetType().Name;

            behaviour?.OnExit();
            behaviour = it;
            behaviour?.OnEnter();
        }

        public void UseAnimation(string it) {
            animator.Play(it);
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
            it.Hurt(damage);
        }

        private void FireProjectile(Vector2 velocity) {
            var instance = Instantiate(projectile, sprite.transform.position, sprite.transform.rotation).GetComponent<SundewProjectile>().Of(Attack);
            var noise = new Vector2(Random.Range(0.25f * -1, 0.25f), Random.Range(0.25f * -1, 0.25f));

            instance.rb.velocity = velocity + noise;
            Destroy(instance.gameObject, windup + reload);
        }
    }
}
