using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Background {
    public class FloatingImage : MonoBehaviour {
        [SerializeField] private float floatSpeed = 0.1f;  // Speed of floating
        [SerializeField] private float floatStrength = 2f;  // How high/low it moves
        [SerializeField] private float fadeDuration = 2f;  // Duration of fade effect
        [SerializeField] private bool fadeOnStart = true;  // Should it fade in?

        private Vector3 startPosition;
        private Image image;
        private float randomOffset;

        private void Start() {
            startPosition = transform.position;
            image = GetComponent<Image>();
            randomOffset = Random.Range(2f * Mathf.PI, 2f * Mathf.PI);

            if (fadeOnStart) {
                StartCoroutine(FadeIn());
            }
        }

        private void Update() {
            // Smooth floating movement using a sine wave
            float xOffset = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
            float yOffset = Mathf.Cos(Time.time * floatSpeed) * floatStrength;
            transform.position = startPosition + new Vector3(xOffset, yOffset, 0);
        }

        public IEnumerator FadeIn() {
            float elapsedTime = 0f;
            Color color = image.color;
            color.a = 0f;
            image.color = color;

            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                image.color = color;
                yield return null;
            }
        }

        public IEnumerator FadeOut() {
            float elapsedTime = 0f;
            Color color = image.color;

            while (elapsedTime < fadeDuration) {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                image.color = color;
                yield return null;
            }
        }
    }

}
