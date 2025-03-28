using UnityEngine;
using UnityEngine.UI; // Required for UI elements

namespace Background {
    public class GlowEffect : MonoBehaviour {
        [SerializeField] private float glowSpeed = 2f;
        [SerializeField] private float minAlpha = 0.3f;
        [SerializeField] private float maxAlpha = 1f;

        private SpriteRenderer glowSprite;
        private Image glowImage;
        private float timeOffset;

        private void Start() {
            glowSprite = GetComponent<SpriteRenderer>();
            glowImage = GetComponent<Image>();

            if (glowSprite == null && glowImage == null) {
                Debug.LogError("GlowEffect: No SpriteRenderer or Image found on " + gameObject.name);
                enabled = false;
                return;
            }

            timeOffset = Random.Range(0f, Mathf.PI * 2f);
        }

        private void Update() {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * glowSpeed + timeOffset) + 1f) / 2f);

            if (glowSprite != null) {
                Color spriteColor = glowSprite.color;
                spriteColor.a = alpha;
                glowSprite.color = spriteColor;
            }

            if (glowImage != null) {
                Color imageColor = glowImage.color;
                imageColor.a = alpha;
                glowImage.color = imageColor;
            }
        }
    }
}
