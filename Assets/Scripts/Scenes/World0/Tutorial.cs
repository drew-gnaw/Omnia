using System;
using System.Collections;
using Enemies.Dummy;
using UI;
using UnityEngine;
using Utils;

namespace Scenes {
    public class Tutorial : MonoBehaviour {
        [SerializeField] private FadeScreenHandler fadeScreen;

        [SerializeField] private Dinky dinky;
        [SerializeField] private GameObject dummy1Obj;
        [SerializeField] private GameObject dummy2Obj;
        [SerializeField] private GameObject dummy3Obj;

        [SerializeField] private ColliderEventBroadcaster dummyAirCheck;

        [SerializeField] private Transform dinkyAppearTransform;

        [SerializeField] private DialogueWrapper beginDialogue;
        [SerializeField] private DialogueWrapper thirdDummyDialogue;
        [SerializeField] private DialogueWrapper pullToDummyDialogue;

        private Dummy dummy1;
        private Dummy dummy2;
        private Dummy dummy3;

        private int dummiesHit = 0;
        private bool beginDialogueTriggered = false;
        private bool thirdDialogueTriggered = false;
        private bool pullToDummyDialogueTriggered = false;

        private void Start() {
            dummy1 = dummy1Obj.GetComponent<Dummy>();
            dummy2 = dummy2Obj.GetComponent<Dummy>();
            dummy3 = dummy3Obj.GetComponent<Dummy>();

            dummy1.OnHurt += HandleDummy1Hurt;
            dummy2.OnHurt += HandleDummy2Hurt;
            dummy3.OnHurt += HandleDummy3Hurt;

            dummyAirCheck.OnEnter += HandleHitAirDummy;

            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            yield return StartCoroutine(fadeScreen.FadeInLightScreen(1f));
            dinky.Appear(dinkyAppearTransform);
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(beginDialogue.Dialogue));
            beginDialogueTriggered = true;

            HighlightManager.Instance.HighlightGameObject(dummy1Obj);
            HighlightManager.Instance.HighlightGameObject(dummy2Obj);
        }

        private void HandleDummy1Hurt() {
            if (!beginDialogueTriggered) return;
            dummy1.OnHurt -= HandleDummy1Hurt;
            HighlightManager.Instance.UnhighlightGameObject(dummy1Obj);
            dummiesHit++;
            if (dummiesHit >= 2 && !thirdDialogueTriggered) {
                thirdDialogueTriggered = true;
                StartCoroutine(ThirdDummySequence());
            }
        }

        private void HandleDummy2Hurt() {
            if (!beginDialogueTriggered) return;
            dummy1.OnHurt -= HandleDummy2Hurt;
            HighlightManager.Instance.UnhighlightGameObject(dummy2Obj);
            dummiesHit++;
            if (dummiesHit >= 2 && !thirdDialogueTriggered) {
                thirdDialogueTriggered = true;
                StartCoroutine(ThirdDummySequence());
            }
        }

        private IEnumerator ThirdDummySequence() {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(thirdDummyDialogue.Dialogue));

            HighlightManager.Instance.HighlightGameObject(dummy3Obj);
        }

        private void HandleDummy3Hurt() {
            if (!thirdDialogueTriggered) return;
            dummy3.OnHurt -= HandleDummy3Hurt;
            StartCoroutine(PullToDummySequence());

        }

        private IEnumerator PullToDummySequence() {
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(pullToDummyDialogue.Dialogue));
            pullToDummyDialogueTriggered = true;
        }

        private void HandleHitAirDummy() {
            if (!pullToDummyDialogueTriggered) return;
            dummyAirCheck.OnEnter -= HandleHitAirDummy;
            StartCoroutine(BeginDummyFallSequence());
        }

        private IEnumerator BeginDummyFallSequence() {
            HighlightManager.Instance.UnhighlightGameObject(dummy3Obj);
            Rigidbody2D rb = dummy3Obj.GetComponent<Rigidbody2D>();
            if (rb == null) {
                rb = dummy3Obj.AddComponent<Rigidbody2D>();
            }

            rb.gravityScale = 1f;
            // always towards the right
            float horizontalForce = UnityEngine.Random.Range(1.5f, 3f);
            rb.velocity = new Vector2(horizontalForce, 0);

            float spinForce = UnityEngine.Random.Range(250f, 400f) * (UnityEngine.Random.value > 0.5f ? 1 : -1);
            rb.angularVelocity = spinForce;

            // dummy hits ground
            yield return new WaitUntil(() => rb.position.y > -4.5f);

            ScreenShakeManager.Instance.Shake(10f);
            Debug.Log("done");
        }


    }
}
