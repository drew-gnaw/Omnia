using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Enemies {
    public abstract class Enemy : MonoBehaviour {
        public static event Action<Enemy> Spawn;
        public static event Action<Enemy> Death;

        [SerializeField] internal float maximumHealth;
        [SerializeField] internal float currentHealth;
        [SerializeField] internal float attack;

        public virtual void Start() {
            currentHealth = maximumHealth;
            Spawn?.Invoke(this);
        }

        public virtual void Hurt(float damage) {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maximumHealth);

            if (currentHealth == 0) Die();
        }

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
