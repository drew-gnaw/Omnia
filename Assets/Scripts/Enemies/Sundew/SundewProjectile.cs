using System;
using Omnia.Utils;
using Players;
using UnityEngine;

namespace Enemies.Sundew {
    public class SundewProjectile : MonoBehaviour {
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask mask;

        private Action<Player> action;

        public void OnTriggerEnter2D(Collider2D other) {
            if (!CollisionUtils.isLayerInMask(other.gameObject.layer, mask)) return;
            if (other.TryGetComponent<Player>(out var player)) action(player);

            Destroy(gameObject);
        }

        public SundewProjectile Of(Action<Player> fn) {
            action = fn;
            return this;
        }
    }
}
