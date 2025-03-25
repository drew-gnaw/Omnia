using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Background {
    public class FloatingImage : MonoBehaviour {
        [SerializeField] private float averageFloatSpeed = 0.2f;
        [SerializeField] private float deltaFloatSpeed = 0.1f;
        [SerializeField] private float floatStrength = 2f;
        [SerializeField] private float fadeDuration = 2f;
        [SerializeField] private bool fadeOnStart = true;

        private float floatSpeed;

        private Vector3 startPosition;
        private Image image;
        private float randomOffset;

        private void Start() {
            startPosition = transform.position;
            image = GetComponent<Image>();

            floatSpeed = deltaFloatSpeed + Random.Range(-deltaFloatSpeed, deltaFloatSpeed);

            if (fadeOnStart) {
                StartCoroutine(FadeIn());
            }
        }

        private void Update() {
            float xOffset = Mathf.Sin(Time.time * floatSpeed + 1) * floatStrength;
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

    }

}
