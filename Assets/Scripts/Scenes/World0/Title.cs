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

        [SerializeField] private TextMeshProUGUI taglineTMP;     // Causa Fiunt (TMP UI)
        [SerializeField] private TextMeshProUGUI quoteTMP;       // Everything happens for a reason (TMP UI)

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
            titleSpriteRenderer = titleSprite.GetComponent<SpriteRenderer>();
            subtitleSpriteRenderer = subtitleSprite.GetComponent<SpriteRenderer>();

            taglineTMP.text = "";
            quoteTMP.text = "";

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
            yield return StartCoroutine(FadeInSprite(subtitleSpriteRenderer, 3f));
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(TypewriterEffect(taglineTMP, "Causa Fiunt", 0.05f));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(TypewriterEffect(quoteTMP, "Everything happens for a reason.", 0.05f));
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

        private IEnumerator TypewriterEffect(TextMeshProUGUI textMesh, string fullText, float delay) {
            textMesh.text = "";
            foreach (char c in fullText) {
                textMesh.text += c;
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
