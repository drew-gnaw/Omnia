using System;
using System.Collections;
using Players;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI {
    public class HUDManager : PersistentSingleton<HUDManager> {
        [SerializeField] private Player player;

        [SerializeField] private Transform healthContainer;
        [SerializeField] private GameObject heartPrefab;

        [SerializeField] private Slider flowSlider;
        private Coroutine flowAnimationCoroutine;


        protected override void OnAwake() {
            gameObject.SetActive(true);
            Player.OnFlowChanged += UpdateFlow;
            Player.OnHealthChanged += UpdateHealth;
        }

        private void OnDisable() {
            Player.OnFlowChanged -= UpdateFlow;
            Player.OnHealthChanged -= UpdateHealth;
        }

        public void UpdateHealth(int currentHealth) {
            int numberOfHearts = (int)Mathf.Ceil(player.maximumHealth / 2);
        }

        public void UpdateFlow(float targetFlow) {
            if (flowAnimationCoroutine != null) {
                StopCoroutine(flowAnimationCoroutine);
            }

            flowAnimationCoroutine = StartCoroutine(SlideFlowValue(targetFlow));
        }

        private IEnumerator SlideFlowValue(float targetFlow) {
            float startValue = flowSlider.value;
            float timeElapsed = 0f;
            float duration = 1f;  // Duration of the animation (1 second)

            while (timeElapsed < duration) {
                float easedValue = EaseOutQuad(timeElapsed / duration);
                flowSlider.value = Mathf.Lerp(startValue, targetFlow, easedValue);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure it ends at the exact target value
            flowSlider.value = targetFlow;
        }

        private float EaseOutQuad(float t) {
            return -t * (t - 2f);
        }

    }
}
