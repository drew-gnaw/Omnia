using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scenes {
    public class Title : LevelSelect {
        [SerializeField] private GameObject omniaText;
        [SerializeField] private GameObject omniaSubtitle;
        [SerializeField] private Button quitButton;

        [SerializeField] private float flickerSpeed = 0.1f;
        [SerializeField] private float flickerIntensity = 0.2f;
        [SerializeField] private Color baseColor = Color.white;
        [SerializeField] private Color flickerColor = new Color(1f, 0.85f, 0.6f);

        private SpriteRenderer titleSpriteRenderer;
        private SpriteRenderer subtitleSpriteRenderer;


        public void QuitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();

        }

        private void Start() {
            titleSpriteRenderer = omniaText.GetComponent<SpriteRenderer>();
            subtitleSpriteRenderer = omniaSubtitle.GetComponent<SpriteRenderer>();

            Color transparentColor = subtitleSpriteRenderer.color;
            transparentColor.a = 0f;
            subtitleSpriteRenderer.color = transparentColor;
        }

        public void StartGame() {
            Debug.Log("starting game...");
            StartCoroutine(FadeInSubtitle(5f));
        }

        void Update() {
            float flicker = flickerSpeed + Random.Range(-flickerIntensity, flickerIntensity);
            titleSpriteRenderer.color = Color.Lerp(baseColor, flickerColor, flicker);
        }

        private IEnumerator FadeInSubtitle(float duration) {
            float elapsedTime = 0f;
            Color startColor = subtitleSpriteRenderer.color;
            Color targetColor = startColor;
            targetColor.a = 1f;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                subtitleSpriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                yield return null;
            }

            subtitleSpriteRenderer.color = targetColor;
        }
    }
}
