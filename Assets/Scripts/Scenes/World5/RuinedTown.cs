using NPC;
using UnityEngine;

namespace Scenes.World5 {
    [RequireComponent(typeof(BoxCollider2D))]
    public class RuinedTown : MonoBehaviour {

        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Uncle uncle;

        private bool uncleTriggered;

        private void OnTriggerEnter2D(Collider2D other) {
            if (((1 << other.gameObject.layer) & playerLayer) != 0) {
                OnPlayerEnter();
            }
        }

        private void OnPlayerEnter() {
            if (uncleTriggered) return;
            Debug.Log("Player entered the uncle's trigger area!");
            uncleTriggered = true;
            uncle.Walk(-5f, 3f);
        }
    }
}
