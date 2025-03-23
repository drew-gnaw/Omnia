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

        [SerializeField] private TextMeshPro PanelText1;
        [SerializeField] private TextMeshPro PanelText2;
        [SerializeField] private TextMeshPro PanelText3;
        [SerializeField] private TextMeshPro PanelText4;

        [SerializeField] private Image Panel1;
        [SerializeField] private Image Panel2;
        [SerializeField] private Image Panel3;

        [SerializeField] private Image Ekey;

        [SerializeField] private float EkeyFadeTime = 1f;
        [SerializeField] private float EkeyDelayTime = 3f;

        private bool canPressE = false;

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
            SetAlpha(PanelText1, 0f);
            SetAlpha(PanelText2, 0f);
            SetAlpha(PanelText3, 0f);
            SetAlpha(PanelText4, 0f);
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
                        StartCoroutine(ShowSecondPanel());
                        break;
                    case 3:
                        StartCoroutine(ShowThirdPanel());
                        break;
                    case 4:
                        StartCoroutine(ExitSequence());
                        break;
                    default:
                        Debug.LogWarning("Hey man we fucked up real bad");
                        break;
                }
                progress++;
            }
        }

        private IEnumerator BeginSequence() {
            StartCoroutine(fadeScreen.FadeInLightScreen(2f));
            panBackground.PanTo(panTarget, 40f);
            yield return Fade(Text1, 1.5f, fadeIn: true);

            yield return new WaitForSeconds(EkeyDelayTime);

            yield return Fade(Ekey, EkeyFadeTime, fadeIn: true);
            canPressE = true;
        }

        private IEnumerator ShowNextText() {
            StartCoroutine(Fade(Ekey, EkeyFadeTime, fadeIn: false));
            yield return Fade(Text1, EkeyFadeTime, fadeIn: false);
            yield return Fade(Text2, 2f, fadeIn: true);

            yield return new WaitForSeconds(EkeyDelayTime);

            yield return Fade(Ekey, EkeyFadeTime, fadeIn: true);
            canPressE = true;
        }

        private IEnumerator HideSecondText() {
            StartCoroutine(Fade(Ekey, EkeyFadeTime, fadeIn: false));
            StartCoroutine(Fade(panBackground.gameObject.GetComponent<SpriteRenderer>(), EkeyFadeTime, fadeIn: false));
            yield return Fade(Text2, EkeyFadeTime, fadeIn: false);

            StartCoroutine(Fade(Panel1, 2f, fadeIn: true));
            yield return Fade(PanelText1, 2f, fadeIn: true);

            yield return new WaitForSeconds(EkeyDelayTime);
            yield return Fade(Ekey, EkeyFadeTime, fadeIn: true);
            canPressE = true;
        }

        private IEnumerator ShowSecondPanel() {
            StartCoroutine(Fade(Ekey, EkeyFadeTime, fadeIn: false));

            StartCoroutine(Fade(Panel2, 2f, fadeIn: true));
            StartCoroutine(Fade(PanelText2, 2f, fadeIn: true));
            yield return Fade(PanelText3, 2f, fadeIn: true);

            yield return new WaitForSeconds(EkeyDelayTime);
            yield return Fade(Ekey, EkeyFadeTime, fadeIn: true);
            canPressE = true;
        }

        private IEnumerator ShowThirdPanel() {
            StartCoroutine(Fade(Ekey, EkeyFadeTime, fadeIn: false));

            StartCoroutine(Fade(PanelText4, 2f, fadeIn: true));
            yield return Fade(Panel3, 2f, fadeIn: true);

            yield return new WaitForSeconds(EkeyDelayTime);
            yield return Fade(Ekey, EkeyFadeTime, fadeIn: true);
            canPressE = true;
        }

        private IEnumerator ExitSequence() {
            yield return StartCoroutine(fadeScreen.FadeInDarkScreen(2f));
            LevelManager.Instance.NextLevel();
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
