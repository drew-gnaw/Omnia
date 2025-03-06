using Enemies;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class EnemyUI : MonoBehaviour {
        [SerializeField] internal RectTransform rect;
        [SerializeField] internal Canvas canvas;
        [SerializeField] internal Slider slider;
        [SerializeField] internal Slider shield;
        [SerializeField] internal Enemy subject;

        private Shield enemyShield;

        public void Start() {
            enemyShield = subject.GetComponent<Shield>();
            shield.gameObject.SetActive(enemyShield != null);
        }

        public void Update() {
            slider.value = subject.maximumHealth == 0 ? 0 : subject.currentHealth / subject.maximumHealth;
            UpdateShield();
        }

        public EnemyUI Of(Enemy e) {
            subject = e;
            return this;
        }

        public void OnCameraUpdate() {
            rect.anchoredPosition = WorldToUIPoint(subject.transform.position, canvas.worldCamera);
        }

        private Vector2 WorldToUIPoint(Vector2 position, Camera c) {
            var p = c.WorldToViewportPoint(position);
            var d = 2 * canvas.scaleFactor;
            return new Vector2(c.pixelWidth * (p.x * 2 - 1) / d, c.pixelHeight * (p.y * 2 - 1) / d);
        }

        private void UpdateShield() {
            if (enemyShield == null) return;
            shield.value = enemyShield.maxShieldHealth == 0 ? 0 : enemyShield.shieldHealth / enemyShield.maxShieldHealth;
        }
    }
}
