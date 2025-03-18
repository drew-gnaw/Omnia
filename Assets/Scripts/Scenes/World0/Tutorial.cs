using System;
using System.Collections;
using UI;
using UnityEngine;
using Utils;

namespace Scenes {
    public class Tutorial : MonoBehaviour {
        [SerializeField] private FadeScreenHandler fadeScreen;

        [SerializeField] private GameObject dummy1;
        [SerializeField] private GameObject dummy2;
        [SerializeField] private GameObject dummy3;

        // DIALOGUE WRAPPERS
        [SerializeField] private DialogueWrapper beginDialogue;

        public void Start() {
            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            yield return StartCoroutine(fadeScreen.FadeInLightScreen(1f));
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(beginDialogue.Dialogue));

            Debug.Log(HighlightManager.Instance);
            HighlightManager.Instance.HighlightGameObject(dummy1);
            HighlightManager.Instance.HighlightGameObject(dummy2);
        }
    }
}
