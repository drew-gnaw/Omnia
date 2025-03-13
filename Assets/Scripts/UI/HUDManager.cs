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
        [SerializeField] private GameObject halfHeartPrefab;
        [SerializeField] private GameObject emptyHeartPrefab;

        [SerializeField] private Transform ammoContainer;
        [SerializeField] private GameObject ammoPrefab;
        [SerializeField] private GameObject emptyAmmoPrefab;

        [SerializeField] private Slider flowSlider;
        private Coroutine flowAnimationCoroutine;


        protected override void OnAwake() {
            gameObject.SetActive(true);

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) {
                player = playerObj.GetComponent<Player>();
            } else {
                Debug.LogError("HUDManager: No GameObject with tag 'Player' found!");
            }

            Player.OnFlowChanged += UpdateFlow;
            Player.OnHealthChanged += UpdateHealth;
        }

        private void OnDisable() {
            Player.OnFlowChanged -= UpdateFlow;
            Player.OnHealthChanged -= UpdateHealth;
        }

        public void UpdateHealth(int currentHealth) {
            foreach (Transform child in healthContainer) {
                Destroy(child.gameObject);
            }

            int maxHearts = player.maximumHealth / 2;
            int fullHearts = currentHealth / 2;
            bool hasHalfHeart = currentHealth % 2 == 1;

            for (int i = 0; i < maxHearts; i++) {
                if (i < fullHearts) {
                    // Add a full heart
                    Instantiate(heartPrefab, healthContainer);
                } else if (hasHalfHeart && i == fullHearts) {
                    // Add a half heart if there's an odd health value
                    Instantiate(halfHeartPrefab, healthContainer);
                } else {
                    // Fill the rest with empty hearts
                    Instantiate(emptyHeartPrefab, healthContainer);
                }
            }
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
