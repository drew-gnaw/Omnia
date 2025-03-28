using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float pause = 0.5f;
    [SerializeField] private float fillDuration = 2.0f;

    public void UpdateBar(float current, float max) {
        slider.value = current / max;
    }

    public void SetColor(Color c) {
        slider.fillRect.GetComponent<Image>().color = c;
    }

    public IEnumerator FadeInAndFill() {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(pause);
        yield return StartCoroutine(AnimateFill());
    }

    private IEnumerator FadeIn() {
        float timer = 0f;
        while (timer < fadeDuration) {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private IEnumerator AnimateFill() {
        float timer = 0f;
        float startValue = 0f;
        float endValue = 1f;

        while (timer < fillDuration) {
            timer += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, endValue, timer / fillDuration);
            yield return null;
        }
        slider.value = endValue;
    }
}
