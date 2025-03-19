using System.Collections;
using Background;
using TMPro;
using UnityEngine;
using Utils;

namespace Scenes {
    public class Opening : MonoBehaviour {
        [SerializeField] private FadeScreenHandler fadeScreen;
        [SerializeField] private PanBackground panBackground;
        [SerializeField] private Transform panTarget;

        [SerializeField] private TextMeshPro Text1;
        [SerializeField] private TextMeshPro Text2;

        private string text1;
        private string text2;

        public void Start() {
            StartCoroutine(BeginSequence());
            Text1.color = new Color(Text1.color.r, Text1.color.g, Text1.color.b, 0f);
            Text2.color = new Color(Text2.color.r, Text2.color.g, Text2.color.b, 0f);
        }

        private IEnumerator BeginSequence() {
            StartCoroutine(fadeScreen.FadeInLightScreen(1f));
            panBackground.PanTo(panTarget, 30f);
            yield return FadeInText(Text1, 1.5f);
            yield return new WaitForSeconds(5f);
            yield return FadeOutText(Text1, 1.5f);
        }

        private IEnumerator FadeOutText(TextMeshPro text, float duration) {
            Color originalColor = text.color;
            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }

        private IEnumerator FadeInText(TextMeshPro text, float duration) {
            Color originalColor = text.color;
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }

            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        }
    }
}
