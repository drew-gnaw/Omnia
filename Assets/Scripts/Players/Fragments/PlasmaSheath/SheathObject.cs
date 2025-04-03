using System;
using Enemies;
using UnityEngine;

namespace Players.Fragments {
    public class SheathObject : MonoBehaviour {
        [SerializeField] private Animator animator;
        public float damage;
        public float velocityThreshold;

        private Player player;
        private bool active;
        private bool wasActive;

        public static readonly int AppearTrigger = Animator.StringToHash("Appear");
        public static readonly int DisappearTrigger = Animator.StringToHash("Disappear");

        private void Start() {
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        }

        private void Update() {
            bool shouldBeActive = player.rb.velocity.y < -velocityThreshold;

            if (shouldBeActive && !wasActive) {
                animator.SetTrigger(AppearTrigger);
            } else if (!shouldBeActive && wasActive) {
                animator.SetTrigger(DisappearTrigger);
            }

            wasActive = shouldBeActive;
            active = shouldBeActive;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (active) {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.Hurt(damage);
                }
            }
        }
    }
}
