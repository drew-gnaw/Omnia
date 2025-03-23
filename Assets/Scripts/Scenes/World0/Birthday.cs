using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Scenes {
    public class Birthday : MonoBehaviour {
        [SerializeField] private FadeScreenHandler fadeScreen;

        [SerializeField] private DialogueWrapper birthdayDialogue;
        void Start() {
            StartCoroutine(BeginSequence());
        }

        private IEnumerator BeginSequence() {
            AudioManager.Instance.SwitchBGM(AudioTracks.JamiesTheme);
            yield return StartCoroutine(fadeScreen.FadeInLightScreen(2f));
            yield return StartCoroutine(DialogueManager.Instance.StartDialogue(birthdayDialogue.Dialogue));
            yield return StartCoroutine(fadeScreen.FadeInDarkScreen(2f));
            LevelManager.Instance.NextLevel();
        }

    }
}



