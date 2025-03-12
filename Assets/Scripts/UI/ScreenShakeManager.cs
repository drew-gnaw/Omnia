using System.Collections;
using UnityEngine;
using Utils;

namespace UI {
    public class ScreenShakeManager : PersistentSingleton<ScreenShakeManager> {
        public static ScreenShakeManager Instance;
        private Vector3 originalPos;

        protected override void OnAwake() {
            Instance = this;
            originalPos = Camera.main.transform.position;
        }

        public void Shake(float duration, float magnitude) {
            StartCoroutine(ShakeRoutine(duration, magnitude));
        }

        private IEnumerator ShakeRoutine(float duration, float magnitude) {
            float elapsed = 0f;
            while (elapsed < duration) {
                float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
                float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;
                Camera.main.transform.position = originalPos + new Vector3(x, y, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
            Camera.main.transform.position = originalPos;
        }
    }

}
