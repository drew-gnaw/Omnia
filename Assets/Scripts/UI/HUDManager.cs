using System;
using System.Collections;
using System.Collections.Generic;
using Players;
using Players.Mixin;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI {
    public class HUDManager : PersistentSingleton<HUDManager> {
        private Player player;

        [SerializeField] private Transform healthContainer;
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private GameObject halfHeartPrefab;
        [SerializeField] private GameObject emptyHeartPrefab;

        [SerializeField] private Slider skillCooldownSlider;

        [SerializeField] private Transform ammoContainer;
        [SerializeField] private GameObject ammoPrefab;
        [SerializeField] private GameObject emptyAmmoPrefab;

        [SerializeField] private Sprite harpoonGunSprite;
        [SerializeField] private Sprite harpoonGunTipSprite;
        [SerializeField] private Sprite shotgunSprite;
        [SerializeField] private Sprite shotgunTipSprite;

        [SerializeField] private Image weaponRenderer;
        [SerializeField] private Image weaponTipRenderer;

        [SerializeField] private Slider flowSlider;
        [SerializeField] private Image lowHealthEffect;
        private Coroutine lowHealthCoroutine;
        private Coroutine flowAnimationCoroutine;

        protected override void OnAwake() {
            gameObject.SetActive(true);

            FindPlayer();

            Player.OnFlowChanged += UpdateFlow;
            Player.OnHealthChanged += UpdateHealth;
            Player.OnWeaponChanged += SetWeaponSprites;
            WeaponClass.OnAmmoChanged += UpdateAmmo;
            Player.OnSkillCooldownUpdated += UpdateSkillCooldown;
        }

        private void OnDisable() {
            Player.OnFlowChanged -= UpdateFlow;
            Player.OnHealthChanged -= UpdateHealth;
            Player.OnWeaponChanged -= SetWeaponSprites;
            WeaponClass.OnAmmoChanged -= UpdateAmmo;
            Player.OnSkillCooldownUpdated -= UpdateSkillCooldown;
        }

        private void UpdateHealth(int currentHealth) {
            foreach (Transform child in healthContainer) {
                Destroy(child.gameObject);
            }

            int maxHearts = player.maximumHealth / 2;
            int fullHearts = currentHealth / 2;
            bool hasHalfHeart = currentHealth % 2 == 1;

            List<GameObject> heartObjects = new List<GameObject>();

            for (int i = 0; i < maxHearts; i++) {
                GameObject heartInstance;

                if (i < fullHearts) {
                    heartInstance = Instantiate(heartPrefab, healthContainer);
                } else if (hasHalfHeart && i == fullHearts) {
                    heartInstance = Instantiate(halfHeartPrefab, healthContainer);
                } else {
                    heartInstance = Instantiate(emptyHeartPrefab, healthContainer);
                }

                heartObjects.Add(heartInstance);
            }

            //StartCoroutine(FlashHearts(heartObjects));
            HandleLowHealthEffect(currentHealth);
        }

        private void UpdateFlow(float targetFlow) {
            if (flowAnimationCoroutine != null) {
                StopCoroutine(flowAnimationCoroutine);
            }

            flowAnimationCoroutine = StartCoroutine(SlideFlowValue(targetFlow));
        }

        private void UpdateAmmo(int currentAmmo) {
            FindPlayer();
            if (!player.weapons[player.selectedWeapon]) {
                Debug.LogWarning("HUDManager: No ammo available for selected weapon.");
            }

            foreach (Transform child in ammoContainer) {
                Destroy(child.gameObject);
            }

            int maxAmmo = player.weapons[player.selectedWeapon].maxAmmoCount;

            for (int i = 0; i < maxAmmo; i++) {
                if (i < currentAmmo) {
                    Instantiate(ammoPrefab, ammoContainer);
                } else {
                    Instantiate(emptyAmmoPrefab, ammoContainer);
                }
            }
        }

        // progress is a float where 0 <= progress <= 1
        private void UpdateSkillCooldown(float progress) {
            skillCooldownSlider.value = progress;
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

        private void SetWeaponSprites(int targetWeapon) {
            UpdateAmmo(player.weapons[player.selectedWeapon].CurrentAmmo);

            switch (targetWeapon) {
                case 0:
                    weaponRenderer.sprite = harpoonGunSprite;
                    weaponTipRenderer.sprite = harpoonGunTipSprite;
                    break;
                case 1:
                    weaponRenderer.sprite = shotgunSprite;
                    weaponTipRenderer.sprite = shotgunTipSprite;
                    break;
                default:
                    Debug.LogWarning("Tried to update HUD to invalid weapon: " + targetWeapon);
                    break;
            }
        }

        private IEnumerator FlashHearts(List<GameObject> hearts) {
            Color originalColor = Color.white;
            Color flashColor = Color.red;

            foreach (GameObject heart in hearts) {
                Image heartImage = heart.GetComponent<Image>();
                if (heartImage != null) {
                    heartImage.color = flashColor;
                }
            }

            yield return new WaitForSeconds(0.1f);  // Short flash duration

            foreach (GameObject heart in hearts) {
                Image heartImage = heart.GetComponent<Image>();
                if (heartImage != null) {
                    heartImage.color = originalColor;
                }
            }
        }

        private void HandleLowHealthEffect(int currentHealth) {
            float targetAlpha = currentHealth <= 2 ? 0.1f : 0f;

            if (lowHealthCoroutine != null) {
                StopCoroutine(lowHealthCoroutine);
            }

            lowHealthCoroutine = StartCoroutine(FadeLowHealthEffect(targetAlpha));
        }

        private IEnumerator FadeLowHealthEffect(float targetAlpha) {
            float startAlpha = lowHealthEffect.color.a;
            float duration = 0.5f;
            float timeElapsed = 0f;

            while (timeElapsed < duration) {
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
                lowHealthEffect.color = new Color(1f, 0f, 0f, newAlpha);  // Red with transparency
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure exact target alpha at the end
            lowHealthEffect.color = new Color(1f, 0f, 0f, targetAlpha);
        }

        private float EaseOutQuad(float t) {
            return -t * (t - 2f);
        }

        private void FindPlayer() {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) {
                player = playerObj.GetComponent<Player>();
            } else {
                Debug.LogError("HUDManager: No GameObject with tag 'Player' found!");
            }
        }

    }
}
