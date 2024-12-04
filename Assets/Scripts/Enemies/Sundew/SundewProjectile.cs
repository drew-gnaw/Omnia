using System;
using Omnia.Utils;
using UnityEngine;

namespace Enemies.Sundew {
    public class SundewProjectile : MonoBehaviour {
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask mask;

        private Action<Player.Alt.Player> action;

        public void OnTriggerEnter2D(Collider2D other) {
            if (!CollisionUtils.isLayerInMask(other.gameObject.layer, mask)) return;
            if (other.TryGetComponent<Player.Alt.Player>(out var player)) action(player);

            Destroy(gameObject);
        }

        public SundewProjectile Of(Action<Player.Alt.Player> fn) {
            action = fn;
            return this;
        }
    }
}
