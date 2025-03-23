using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Scenes {
    public class Title : LevelSelect {
        [SerializeField] private GameObject titleSprite;         // Omnia (Sprite)
        [SerializeField] private GameObject subtitleSprite;      // The Journey Upwards (Sprite)

        [SerializeField] private TextMeshPro taglineTMP;     // Causa Fiunt (TMP UI)
        [SerializeField] private TextMeshPro quoteTMP;       // Everything happens for a reason (TMP UI)

        [SerializeField] private FadeScreenHandler strongFadeHandler;
        [SerializeField] private float flickerSpeed = 0.1f;
        [SerializeField] private float flickerIntensity = 0.2f;
        [SerializeField] private Color baseColor = Color.white;
        [SerializeField] private Color flickerColor = new Color(1f, 0.85f, 0.6f);

        private SpriteRenderer titleSpriteRenderer;
        private SpriteRenderer subtitleSpriteRenderer;

        private string taglineText;
        private string quoteText;



        public void QuitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();

        }

        private void Awake() {
            base.Awake();
            strongFadeHandler.SetDarkScreen();
            StartCoroutine(strongFadeHandler.FadeInLightScreen(1f));
        }

        private void Start() {
            titleSpriteRenderer = titleSprite.GetComponent<SpriteRenderer>();
            subtitleSpriteRenderer = subtitleSprite.GetComponent<SpriteRenderer>();

            // Text is not displayed at the beginning, but we should store their values.
            taglineText = taglineTMP.text;
            taglineTMP.text = "";

            quoteText = quoteTMP.text;
            quoteTMP.text = "";

            AudioManager.Instance.PlayBGM(AudioTracks.LullabyForAScrapyard);

        }

        public void StartGame() {
            foreach (Button b in buttons)
            {
                b.interactable = false;
            }
            StartCoroutine(StartGameSequence());
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

        void Update() {
            float flicker = flickerSpeed + Random.Range(-flickerIntensity, flickerIntensity);
            titleSpriteRenderer.color = Color.Lerp(baseColor, flickerColor, flicker);
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
