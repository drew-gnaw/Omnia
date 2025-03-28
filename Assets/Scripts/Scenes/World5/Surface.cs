using UnityEngine;
using Utils;

namespace Scenes.World5 {
    public class Surface : MonoBehaviour {
        [SerializeField] private FloatCamera camera;
        [SerializeField] private FadeScreenHandler fadeScreen;

        public void Start() {
            fadeScreen.SetDarkScreen();
            StartCoroutine(fadeScreen.FadeInLightScreen(2f));
            camera.StartFloating();
        }
    }
}
