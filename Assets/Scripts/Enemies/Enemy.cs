using System;
using System.Linq;
using UnityEngine;
using Omnia.State;
using Enemies.Common.Behaviour;

namespace Enemies {
    public abstract class Enemy : MonoBehaviour {
        public static event Action<Enemy> Spawn;
        public static event Action<Enemy> Death;
        public Func<float, float> OnHurt;

        [SerializeField] internal float maximumHealth;
        [SerializeField] internal float currentHealth;
        [SerializeField] internal float attack;
        [SerializeField] internal float knockbackForce = 10f;
        [SerializeField] internal float knockbackAngle = 45f;
        [SerializeField] internal float staggerDurationS = 1f;

        protected IBehaviour behaviour;
        public IBehaviour prevBehaviour { get; protected set; }

        protected StateMachine animationStateMachine;

        public virtual void Start() {
            currentHealth = maximumHealth;
            Spawn?.Invoke(this);
            UseAnimation(new StateMachine());
        }

        public virtual void Hurt(float damage) {
            if (OnHurt != null) damage = OnHurt.Invoke(damage);
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maximumHealth);

            if (currentHealth == 0) Die();

            // Currently the enemy just attempts its previous behaviour after stagger
            if (staggerDurationS <= 0) return;
            // Also prevent staggering if damage is 0
            if (damage == 0) return;

            // Previous behavious should be set here to avoid softlocking the enemy
            prevBehaviour = behaviour;
            UseBehaviour(Stagger.If(this));
        }

        /**
         * The child inheriting this class must call base.Update()
         */
        public virtual void Update() {
            behaviour?.OnUpdate();
            animationStateMachine?.Update();
        }

        /**
         * The child inheriting this class must call base.FixedUpdate()
         */
        public virtual void FixedUpdate() {
            behaviour?.OnTick();
            animationStateMachine?.FixedUpdate();
        }

        public virtual void UseBehaviour(IBehaviour it) {
            behaviour?.OnExit();
            behaviour = it;
            behaviour?.OnEnter();
        }

        /**
         * This method should define the animationStateMachine, which is then updated in Update and FixedUpdate.
         */
        protected abstract void UseAnimation(StateMachine stateMachine);

        /* TODO: This could be a coroutine so enemies can play an animation on death...? */
        private void Die() {
            Death?.Invoke(this);
            Destroy(gameObject);
        }

        /* TODO: These methods can be moved to Utils if needed. */
        public static RaycastHit2D[] Sweep(Vector2 origin, Vector2 direction, float angle, float distance, int count, LayerMask mask) {
            var step = count == 1 ? 0 : angle / (count - 1);
            var initial = angle / 2f;
            return Enumerable.Range(0, count).Select(it => Raycast(origin, direction, initial - step * it, distance, mask)).ToArray();
        }

        public static bool IsOnLayer(RaycastHit2D hit, LayerMask mask) {
            return hit && (mask & 1 << hit.collider.gameObject.layer) != 0;
        }

        private static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float angle, float distance, LayerMask mask) {
            /* TODO: Remove debug. */
            Debug.DrawRay(origin, Quaternion.Euler(0, 0, angle) * direction.normalized * distance);

            return Physics2D.Raycast(origin, Quaternion.Euler(0, 0, angle) * direction.normalized, distance, mask);
        }
    }
}
