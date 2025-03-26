using System;
using System.Collections;
using Background;
using Initializers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Scenes {
    public class Title : MonoBehaviour {
        [SerializeField] private GameObject titleSprite;         // Omnia (Sprite)
        [SerializeField] private GameObject subtitleSprite;      // The Journey Upwards (Sprite)

        [SerializeField] private TextMeshPro taglineTMP;     // Causa Fiunt (TMP UI)
        [SerializeField] private TextMeshPro quoteTMP;       // Everything happens for a reason (TMP UI)

        [SerializeField] private FadeScreenHandler fadeScreen;
        [SerializeField] private FadeScreenHandler strongFadeHandler;

        [SerializeField] private GameObject dustParent;

        [SerializeField] private Button[] buttons;


        private Image[] dustImages;

        private string taglineText;
        private string quoteText;





        public void QuitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        private void Awake() {
            strongFadeHandler.SetDarkScreen();
            StartCoroutine(strongFadeHandler.FadeInLightScreen(1f));
        }

        private void Start() {
            dustImages = dustParent.GetComponentsInChildren<Image>();

            foreach (var img in dustImages) {
                img.gameObject.AddComponent<FloatingImage>();
            }

            // Text is not displayed at the beginning, but we should store their values.
            taglineText = taglineTMP.text;
            taglineTMP.text = "";

            quoteText = quoteTMP.text;
            quoteTMP.text = "";

            AudioManager.Instance.PlayBGM(AudioTracks.LullabyForAScrapyard);
            fadeScreen.SetDarkScreen();
            StartCoroutine(fadeScreen.FadeInLightScreen(1f));

        }

        public void StartGame() {
            foreach (Button b in buttons)
            {
                b.interactable = false;
            }
            StartCoroutine(StartGameSequence());
        }

        public void GoToLevelSelect() {
            SceneInitializer.LoadScene("LevelSelect");
        }

        public void Update() {
            Debug.Log(buttons);
        }

        private IEnumerator StartGameSequence() {
            yield return StartCoroutine(fadeScreen.FadeInDarkScreen(2f));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(Typewriter.TypewriterEffect(taglineTMP, taglineText, 0.15f));
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(Typewriter.TypewriterEffect(quoteTMP, quoteText, 0.1f));
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(strongFadeHandler.FadeInDarkScreen(2f));

            // Done to avoid scene transition
            SceneManager.LoadScene("2_Opening");
        }

        private IEnumerator FadeInSprite(SpriteRenderer sprite, float duration) {
            float elapsedTime = 0f;
            Color startColor = sprite.color;
            Color targetColor = startColor;
            targetColor.a = 1f;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                sprite.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                yield return null;
            }

            sprite.color = targetColor;
        }


    }
}
