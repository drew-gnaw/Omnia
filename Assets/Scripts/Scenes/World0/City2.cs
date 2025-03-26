using System;
using System.Collections;
using NPC;
using UnityEngine;
using Utils;

namespace Scenes {
    public class City2 : MonoBehaviour {
        [SerializeField] private FadeScreenHandler fadeScreen;
        public void Start() {
            CrazyOldMan.OnSpeakToCrazyOldMan += PlayExitSequence;
            AudioManager.Instance.SwitchBGM(AudioTracks.CityOfMold);
            fadeScreen.SetLightScreen();
        }

        public void OnDisable() {
            CrazyOldMan.OnSpeakToCrazyOldMan -= PlayExitSequence;
        }

        private void PlayExitSequence() {
            StartCoroutine(ExitSequence());
        }

        private IEnumerator ExitSequence() {
            yield return StartCoroutine(fadeScreen.FadeInDarkScreen(2f));
            LevelManager.Instance.NextLevel();
        }
    }
}
