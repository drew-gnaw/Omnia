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


        public void QuitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();

        }

        private void Start() {
            quitButton.interactable = false;
            titleSpriteRenderer = omniaText.GetComponent<SpriteRenderer>();
        }

        public void StartGame() {
            Debug.Log("starting game...");
        }


        private float cycleScaling = 2f; // Higher the number, the faster one phase is
        private float bobbingAmount = 0.1f; //Amplitude
        private float timer = 0;
        private float verticalOffset = 0;

        //Makes the game over text bob up and down!
        void Update() {
            float flicker = 1f + Random.Range(-flickerIntensity, flickerIntensity);
            titleSpriteRenderer.color = Color.Lerp(baseColor, flickerColor, flicker);
        }
    }
}
