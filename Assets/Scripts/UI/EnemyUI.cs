using Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class EnemyUI : MonoBehaviour {
        [SerializeField] internal Slider slider;
        [SerializeField] internal Enemy target;

        public void Update() {
            slider.value = target.maximumHealth == 0 ? 0 : target.currentHealth / target.maximumHealth;
        }

        public EnemyUI Of(Enemy it) {
            target = it;
            return this;
        }

        public void Move(Vector2 it) {
            var rect = transform as RectTransform;
            if (rect) rect!.anchoredPosition = it;
        }
    }
}
