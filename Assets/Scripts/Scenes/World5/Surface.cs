using System.Collections;
using UnityEngine;
using Utils;

namespace Scenes.World5 {
    public class Surface : MonoBehaviour {
        [SerializeField] private FloatCamera camera;
        [SerializeField] private FadeScreenHandler fadeScreen;

        public void Start() {
            StartCoroutine(StartSequence());
        }

        private IEnumerator StartSequence() {
            fadeScreen.SetDarkScreen();
            yield return StartCoroutine(fadeScreen.FadeInLightScreen(3f));

            camera.StartFloating();
        }
    }
}
