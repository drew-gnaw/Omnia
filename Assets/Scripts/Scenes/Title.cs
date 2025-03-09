using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Scenes {
    public class Title : LevelSelect {
        [SerializeField] private GameObject omniaText;
        [SerializeField] private GameObject omniaSubtitle;
        [SerializeField] private GameObject omniaSubtitle2;
        [SerializeField] private Button quitButton;

        [SerializeField] private float flickerSpeed = 0.1f;
        [SerializeField] private float flickerIntensity = 0.2f;
        [SerializeField] private Color baseColor = Color.white;
        [SerializeField] private Color flickerColor = new Color(1f, 0.85f, 0.6f);

        private SpriteRenderer titleSpriteRenderer;
        private SpriteRenderer subtitleSpriteRenderer;
        private SpriteRenderer subtitle2SpriteRenderer;

        // TITLE:       Omnia
        // SUBTITLE:    The Journey Upwards
        // SUBTITLE2:   Everything happens for a reason


        public void QuitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();

        }

        private void Start() {
            titleSpriteRenderer = omniaText.GetComponent<SpriteRenderer>();
            subtitleSpriteRenderer = omniaSubtitle.GetComponent<SpriteRenderer>();
            subtitle2SpriteRenderer = omniaSubtitle2.GetComponent<SpriteRenderer>();

            Color transparentColor = subtitleSpriteRenderer.color;
            transparentColor.a = 0f;
            subtitleSpriteRenderer.color = transparentColor;
            subtitle2SpriteRenderer.color = transparentColor;
        }

        public void StartGame() {
            Debug.Log(buttons);
            StartCoroutine(StartGameSequence());
        }

        private IEnumerator StartGameSequence() {
            yield return StartCoroutine(fadeScreen.FadeInDarkScreen(2f));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(FadeInSprite(subtitleSpriteRenderer, 3f));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(FadeInSprite(subtitle2SpriteRenderer, 3f));
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("MainScene");
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
