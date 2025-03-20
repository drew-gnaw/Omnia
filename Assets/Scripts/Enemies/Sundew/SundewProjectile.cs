using System;
using Omnia.Utils;
using Players;
using UnityEngine;

namespace Enemies.Sundew {
    public class SundewProjectile : MonoBehaviour {
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask mask;
        [SerializeField] internal float time;

        public Action<Player, SundewProjectile> NotifyOnHit;

        public void Start() => Destroy(gameObject, time);

        public void OnTriggerEnter2D(Collider2D other) {
            if (!CollisionUtils.IsLayerInMask(other.gameObject.layer, mask)) return;
            Destroy(gameObject);

            if (other.TryGetComponent<Player>(out var player)) NotifyOnHit?.Invoke(player, this);
        }
    }
}
