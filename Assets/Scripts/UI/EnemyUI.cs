using Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class EnemyUI : MonoBehaviour {
        [SerializeField] internal RectTransform rect;
        [SerializeField] internal Canvas canvas;
        [SerializeField] internal Slider slider;

        [SerializeField] internal Enemy subject;

        public void Update() {
            slider.value = subject.maximumHealth == 0 ? 0 : subject.currentHealth / subject.maximumHealth;
        }

        public EnemyUI Of(Enemy e) {
            subject = e;
            return this;
        }

        public void OnCameraUpdate() {
            rect.anchoredPosition = WorldToUIPoint(subject.transform.position, canvas.worldCamera);
        }

        private static Vector2 WorldToUIPoint(Vector2 position, Camera camera) {
            var p = camera.WorldToViewportPoint(position);
            return new Vector2(camera.scaledPixelWidth * (p.x * 2 - 1) / 2, camera.scaledPixelHeight * (p.y * 2 - 1) / 2);
        }
    }
}
