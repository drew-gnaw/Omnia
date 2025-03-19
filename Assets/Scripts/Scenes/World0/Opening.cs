using System.Collections;
using Background;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Scenes {
    public class Opening : MonoBehaviour {
        [SerializeField] private FadeScreenHandler fadeScreen;
        [SerializeField] private PanBackground panBackground;
        [SerializeField] private Transform panTarget;

        [SerializeField] private TextMeshPro Text1;
        [SerializeField] private TextMeshPro Text2;

        [SerializeField] private Image Panel1;
        [SerializeField] private Image Panel2;
        [SerializeField] private Image Panel3;


        [SerializeField] private Image Ekey;

        private bool canPressE = false;
        private float EkeyFadeTime = 1f;
        private int progress = 0;
        // Progress is an integer representing how far in the scene we are.
        // 0 = Looking at text1
        // 1 = Looking at text2
        // 2 = Looking at panel1
        // 3 = Looking at panel2
        // 3 = Looking at panel3

        public void Start() {
            StartCoroutine(BeginSequence());
            SetAlpha(Text1, 0f);
            SetAlpha(Text2, 0f);
            SetAlpha(Ekey, 0f);
            SetAlpha(Panel1, 0f);
            SetAlpha(Panel2, 0f);
            SetAlpha(Panel3, 0f);
        }

        private void Update() {
            if (canPressE && Input.GetKeyDown(KeyCode.E)) {
                canPressE = false;
                switch (progress) {
                    case 0:
                        StartCoroutine(ShowNextText());
                        break;
                    case 1:
                        StartCoroutine(HideSecondText());
                        break;
                    case 2:
                        break;
                }
                progress++;
            }
        }

        private IEnumerator BeginSequence() {
            StartCoroutine(fadeScreen.FadeInLightScreen(1f));
            panBackground.PanTo(panTarget, 30f);
            yield return Fade(Text1, 1.5f, fadeIn: true);

            yield return new WaitForSeconds(5f);

            yield return Fade(Ekey, EkeyFadeTime, fadeIn: true);
            canPressE = true;
        }

        private IEnumerator ShowNextText() {
            StartCoroutine(Fade(Ekey, EkeyFadeTime, fadeIn: false));
            yield return Fade(Text1, 2f, fadeIn: false);
            yield return Fade(Text2, 2f, fadeIn: true);

            yield return new WaitForSeconds(5f);

            yield return Fade(Ekey, EkeyFadeTime, fadeIn: true);
            canPressE = true;
        }

        private IEnumerator HideSecondText() {
            StartCoroutine(Fade(Ekey, EkeyFadeTime, fadeIn: false));
            StartCoroutine(Fade(panBackground.gameObject.GetComponent<SpriteRenderer>(), 2f, fadeIn: false));
            yield return Fade(Text2, 2f, fadeIn: false);

            yield return Fade(Panel1, 2f, fadeIn: true);

            yield return new WaitForSeconds(5f);
            yield return Fade(Ekey, EkeyFadeTime, fadeIn: true);
            canPressE = true;
        }

        private IEnumerator Fade(Object obj, float duration, bool fadeIn) {
            Color color = GetColor(obj);
            float startAlpha = fadeIn ? 0f : 1f;
            float endAlpha = fadeIn ? 1f : 0f;
            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                SetAlpha(obj, alpha);
                yield return null;
            }

            SetAlpha(obj, endAlpha);
        }

        private Color GetColor(Object obj) {
            if (obj is TextMeshPro text) return text.color;
            if (obj is SpriteRenderer sprite) return sprite.color;
            if (obj is Image image) return image.color; // 🔹 Now supports UI Images
            return Color.white;
        }

        private void SetAlpha(Object obj, float alpha) {
            if (obj is TextMeshPro text)
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            if (obj is SpriteRenderer sprite)
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            if (obj is Image image) // 🔹 Properly update Image transparency
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }

        private IEnumerator FlashEKey() {
            if (Ekey != null) {
                Color originalColor = Ekey.color;

                Ekey.color = Color.yellow;
                yield return new WaitForSeconds(0.2f);
                Ekey.color = originalColor;
            }
        }
    }
}
