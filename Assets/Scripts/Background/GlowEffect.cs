using UnityEngine;

namespace Background {

    public class GlowEffect : MonoBehaviour {
        [SerializeField] private float glowSpeed = 2f;
        [SerializeField] private float minAlpha = 0.3f;
        [SerializeField] private float maxAlpha = 1f;

        private SpriteRenderer glowSprite;
        private float timeOffset;

        private void Start() {
            glowSprite = GetComponent<SpriteRenderer>();

            if (glowSprite == null) {
                Debug.LogError("GlowEffect: No SpriteRenderer found on " + gameObject.name);
                enabled = false;
                return;
            }

            timeOffset = Random.Range(0f, Mathf.PI * 2f);
        }

        private void Update() {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * glowSpeed + timeOffset) + 1f) / 2f);
            Color color = glowSprite.color;
            color.a = alpha;
            glowSprite.color = color;
        }
    }

}
