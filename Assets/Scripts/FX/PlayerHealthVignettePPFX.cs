using Players;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Utils;

namespace FX {
    public class PlayerHealthVignettePPFX : MonoBehaviour {
        [SerializeField] internal PostProcessVolume volume;
        [SerializeField] internal int hpThreshold;
        [SerializeField] internal float fadeSpeed;
        [SerializeField] internal Color hurtColor;
        [SerializeField] internal float hurtIntensity;

        private Vignette vignette;
        private Color normalColor;
        private Color targetColor;
        private float normalIntensity;
        private float targetIntensity;

        public void Awake() {
            vignette = volume.profile.GetSetting<Vignette>();
            normalColor = vignette.color.value;
            normalIntensity = vignette.intensity.value;
        }

        public void OnEnable() {
            Player.OnHealthChanged += OnPlayerHurt;
        }

        public void OnDisable() {
            Player.OnHealthChanged -= OnPlayerHurt;
        }

        private void OnPlayerHurt(int health) {
            targetIntensity = health > hpThreshold ? normalIntensity : hurtIntensity;
            targetColor = health > hpThreshold ? normalColor : hurtColor;
        }

        public void Update() {
            vignette.color.value = MathUtils.Lerpish(vignette.color.value, targetColor, Time.deltaTime * fadeSpeed);
            vignette.intensity.value = MathUtils.Lerpish(vignette.intensity.value, targetIntensity, Time.deltaTime * fadeSpeed);
        }
    }
}
