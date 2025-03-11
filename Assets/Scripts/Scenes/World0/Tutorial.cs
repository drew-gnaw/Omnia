using System;
using System.Collections;
using Enemies.Dummy;
using UI;
using UnityEngine;
using Utils;

namespace Scenes {
    public class Tutorial : MonoBehaviour {
        [SerializeField] private FadeScreenHandler fadeScreen;

        [SerializeField] private GameObject dummy1Obj;
        [SerializeField] private GameObject dummy2Obj;
        [SerializeField] private GameObject dummy3Obj;

        [SerializeField] private DialogueWrapper beginDialogue;
        [SerializeField] private DialogueWrapper thirdDummyDialogue;

        private Dummy dummy1;
        private Dummy dummy2;
        private Dummy dummy3;

        private int dummiesHit = 0;
        private bool thirdDialogueTriggered = false;

        private void Start() {
            dummy1 = dummy1Obj.GetComponent<Dummy>();
            dummy2 = dummy2Obj.GetComponent<Dummy>();
            dummy3 = dummy3Obj.GetComponent<Dummy>();

            dummy1.OnHurt += HandleDummy1Hurt;
            dummy2.OnHurt += HandleDummy2Hurt;

            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            yield return StartCoroutine(fadeScreen.FadeInLightScreen(1f));
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(beginDialogue.Dialogue));

            HighlightManager.Instance.HighlightGameObject(dummy1Obj);
            HighlightManager.Instance.HighlightGameObject(dummy2Obj);
        }

        private void HandleDummy1Hurt() {
            dummy1.OnHurt -= HandleDummy1Hurt;
            HighlightManager.Instance.UnhighlightGameObject(dummy1Obj);
            dummiesHit++;
            if (dummiesHit >= 2 && !thirdDialogueTriggered) {
                thirdDialogueTriggered = true;
                StartCoroutine(ThirdDummySequence());
            }
        }

        private void HandleDummy2Hurt() {
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
        }
    }
}
