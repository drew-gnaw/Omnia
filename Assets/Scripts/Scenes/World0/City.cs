using System;
using System.Collections;
using NPC;
using UnityEngine;
using Utils;

namespace Scenes {
    public class City : MonoBehaviour {
        public FadeScreenHandler fadeScreen;
        public void Start() {
            Guard.OnSpeakToGuard += ExitScene;
            StartCoroutine(fadeScreen.FadeInLightScreen(2f));
        }

        public void OnDisable() {
            Guard.OnSpeakToGuard -= ExitScene;
        }

        private void ExitScene() {
            StartCoroutine(Exit());
        }

        private IEnumerator Exit() {
            yield return fadeScreen.FadeInDarkScreen(3f);
        }
    }
}
