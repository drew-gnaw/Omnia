using Enemies;
using UnityEngine;

namespace Players.Fragments {
    public class Orbital : MonoBehaviour {
        public float damage;

        void OnTriggerEnter2D(Collider2D other) {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.Hurt(damage);
            }
        }
    }
}
