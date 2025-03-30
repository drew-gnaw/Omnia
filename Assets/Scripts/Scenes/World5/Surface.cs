using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using Utils;

namespace Scenes.World5 {
    public class Surface : MonoBehaviour {
        [SerializeField] private FloatCamera floatCamera;
        [SerializeField] private FadeScreenHandler fadeScreen;

        // this dialogue will be shown in a non-skippable format.
        [SerializeField] private List<DialogueText> dialogue;

        [SerializeField] private TextMeshPro jamieTextBox;
        [SerializeField] private TextMeshPro dinkyTextBox;

        public void Start() {
            HUDManager.Instance.gameObject.SetActive(false);
            StartCoroutine(StartSequence());
        }

        private IEnumerator StartSequence() {
            fadeScreen.SetDarkScreen();
            StartCoroutine(fadeScreen.FadeInLightScreen(3f));

            yield return new WaitForSeconds(2f);
            StartCoroutine(PlayDialogue());
            yield return new WaitForSeconds(2f);
            floatCamera.StartFloating();
        }

        private IEnumerator PlayDialogue() {
            foreach (var dialogueText in dialogue) {
                TextMeshPro targetTextBox = null;

                // Determine the target text box
                if (dialogueText.SpeakerName == "Jamie") {
                    targetTextBox = jamieTextBox;
                } else if (dialogueText.SpeakerName == "Dinky") {
                    targetTextBox = dinkyTextBox;
                }

                if (targetTextBox != null) {
                    yield return StartCoroutine(Typewriter.TypewriterEffect(targetTextBox, dialogueText.BodyText, 0.05f));
                    yield return new WaitForSeconds(2f);
                }
            }

            yield return new WaitForSeconds(3f);
            LevelManager.Instance.NextLevel();

        }

    }
}
