using System.Collections;
using Players;
using Players.Mixin;
using UnityEngine;

namespace Scenes.World5 {
    public class Lab_Prefight : MonoBehaviour {
        [SerializeField] private Player jamie;
        [SerializeField] private GameObject dinky;
        [SerializeField] private GameObject tankDinky;

        [SerializeField] DialogueWrapper beginDialogue;
        [SerializeField] DialogueWrapper throwDialogue;
        [SerializeField] DialogueWrapper DinkyThrownDialogue;

        public void Start() {
            Player.controlsLocked = true;
            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(beginDialogue.Dialogue));

            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(throwDialogue.Dialogue));
            yield return new WaitForSeconds(0.5f);

            Rigidbody2D rb = dinky.GetComponent<Rigidbody2D>();
            if (rb == null) {
                rb = dinky.AddComponent<Rigidbody2D>();
            }

            rb.gravityScale = 1f;
            float horizontalForce = 0.3f;
            rb.velocity = new Vector2(horizontalForce, 16);

            float spinForce = UnityEngine.Random.Range(250f, 400f) * (UnityEngine.Random.value > 0.5f ? 1 : -1);
            rb.angularVelocity = spinForce;

            yield return new WaitUntil(() => rb.velocity.y < -1f);

            Destroy(dinky);

            tankDinky.AddComponent<Rigidbody2D>();
            Player.controlsLocked = false;
        }
    }
}
