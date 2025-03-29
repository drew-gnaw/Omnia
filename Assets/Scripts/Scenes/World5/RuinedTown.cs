using System.Collections;
using NPC;
using Players;
using UnityEngine;

namespace Scenes.World5 {
    [RequireComponent(typeof(BoxCollider2D))]
    public class RuinedTown : MonoBehaviour {

        [SerializeField] private LayerMask playerLayer;

        [SerializeField] private Player player;
        [SerializeField] private Uncle uncle;

        [SerializeField] private DialogueWrapper beginDialogue;
        [SerializeField] private DialogueWrapper uncleDialogue;

        private bool uncleTriggered;

        public void Start() {
            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(beginDialogue.Dialogue));
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (((1 << other.gameObject.layer) & playerLayer) != 0) {
                if (!uncleTriggered) {
                    StartCoroutine(OnPlayerEnter());
                }
            }
        }

        private IEnumerator OnPlayerEnter() {
            Player.controlsLocked = true;
            uncleTriggered = true;
            uncle.Walk(-5f, 3f);
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(uncleDialogue.Dialogue));
        }
    }
}
