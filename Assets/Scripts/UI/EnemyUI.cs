using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class EnemyUI : MonoBehaviour {
        [SerializeField] internal EnemyBase target;
        [SerializeField] internal Slider slider;

        public void Update() {
            slider.value = target.currentHealth * 1f / target.maxHealth;
        }

        public EnemyUI Of(EnemyBase it) {
            target = it;
            return this;
        }

        public void Move(Vector2 it) {
            var rect = transform as RectTransform;
            if (rect) rect!.anchoredPosition = it;
        }
    }
}
