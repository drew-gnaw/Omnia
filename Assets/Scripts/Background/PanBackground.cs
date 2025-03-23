using UnityEngine;
using System.Collections;

namespace Background {
    public class PanBackground : MonoBehaviour {
        public float defaultDuration = 1.5f;
        private Coroutine panCoroutine;

        public void PanTo(Transform target, float duration = -1f) {
            if (target == null) {
                Debug.LogWarning("PanTo: Target is null!");
                return;
            }

            if (duration <= 0) {
                duration = defaultDuration;
            }

            if (panCoroutine != null) {
                StopCoroutine(panCoroutine);
            }
            panCoroutine = StartCoroutine(PanCoroutine(target.position, duration));
        }

        private IEnumerator PanCoroutine(Vector3 targetPos, float duration) {
            Vector3 startPos = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                transform.position = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }

            transform.position = targetPos;
            panCoroutine = null;
        }

        // Ease In-Out Cubic for smooth acceleration and deceleration
        private float EaseInOutCubic(float t) {
            return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3) / 2f;
        }
    }
}
