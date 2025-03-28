using System.Collections;
using UI;
using UnityEngine;
using Utils;

namespace Scenes.World5 {
    public class Surface : MonoBehaviour {
        [SerializeField] private FloatCamera camera;
        [SerializeField] private FadeScreenHandler fadeScreen;

        public void Start() {
            HUDManager.Instance.gameObject.SetActive(false);
            StartCoroutine(StartSequence());
        }

        private IEnumerator StartSequence() {
            fadeScreen.SetDarkScreen();
            yield return StartCoroutine(fadeScreen.FadeInLightScreen(3f));

            camera.StartFloating();
        }
    }
}
