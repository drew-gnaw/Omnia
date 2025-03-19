using System;
using UnityEngine;

namespace Utils {
    public class ColliderEventBroadcaster : MonoBehaviour {
        public event Action OnEnter;

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
                OnEnter?.Invoke();
            }
        }

    }
}
