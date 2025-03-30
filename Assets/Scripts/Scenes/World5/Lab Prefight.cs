using System.Collections;
using Players;
using Players.Mixin;
using UnityEngine;

namespace Scenes.World5 {
    public class Lab_Prefight : MonoBehaviour {
        [SerializeField] private Player jamie;
        [SerializeField] private GameObject dinky;
        [SerializeField] private GameObject tankDinky;
        [SerializeField] private GameObject boss;

        [SerializeField] DialogueWrapper beginDialogue;
        [SerializeField] DialogueWrapper dinkyThrownDialogue;
        [SerializeField] DialogueWrapper dinkyHurtDialogue;
        public void Start() {
            Player.controlsLocked = true;
            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(beginDialogue.Dialogue));

            yield return new WaitForSeconds(1f);
            Rigidbody2D rb = dinky.GetComponent<Rigidbody2D>();
            if (rb == null) {
                rb = dinky.AddComponent<Rigidbody2D>();
            }

            rb.gravityScale = 1f;
            float horizontalForce = 0.3f;
            rb.velocity = new Vector2(horizontalForce, 16);

            float spinForce = Random.Range(250f, 400f) * (Random.value > 0.5f ? 1 : -1);
            rb.angularVelocity = spinForce;

            yield return new WaitUntil(() => rb.velocity.y < -1f);

            Destroy(dinky);

            tankDinky.AddComponent<Rigidbody2D>();

            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(dinkyThrownDialogue.Dialogue));

            // machine starts

            boss.AddComponent<BobbingBehaviour>();

            yield return new WaitForSeconds(3f);

            // something goes wrong

            AudioManager.Instance.PlaySFX(AudioTracks.DinkyScream);

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(BlareAlarm());

            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(dinkyHurtDialogue.Dialogue));

            Player.controlsLocked = false;
            LevelManager.Instance.NextLevel();
        }

        private IEnumerator BlareAlarm() {
            while (true) {
                AudioManager.Instance.PlaySFX(AudioTracks.MachineMalfunction);
                yield return new WaitForSeconds(7f);
            }
        }
    }

}
