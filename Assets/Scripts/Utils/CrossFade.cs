using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFade : MonoBehaviour {
#nullable enable
    public struct CrossFadeRenderers {
        public List<SpriteRenderer> frontRenderer;
        public SpriteRenderer backRenderer;

        public CrossFadeRenderers(List<SpriteRenderer> frontRenderer, SpriteRenderer backRenderer) {
            this.frontRenderer = frontRenderer;
            this.backRenderer = backRenderer;
        }
    }
    private Coroutine? currentCrossFade;
    private void AbortCrossFade() {
        if (currentCrossFade != null) StopCoroutine(currentCrossFade);
    }

    public void StartCrossFadeBackground(CrossFadeRenderers renderers, Sprite sprite, float duration = 1f) {
        AbortCrossFade(); // Abort if cross fade is currently animating
        Debug.Log("Starting the cross fade");
        currentCrossFade = StartCoroutine(CrossFadeBackground(renderers, sprite, duration));
    }

    private IEnumerator CrossFadeBackground(CrossFadeRenderers renderers, Sprite comingIn, float duration) {
        if (renderers.frontRenderer.Count == 0) yield break;
        var prevSprite = renderers.frontRenderer[0].sprite;

        if (comingIn == prevSprite) yield break;
        float prevAlpha = renderers.backRenderer.color.a;

        renderers.backRenderer.sprite = prevSprite;
        renderers.frontRenderer.ForEach(it => it.sprite = comingIn);

        float elapsedTime = prevAlpha;

        while (elapsedTime < duration) {
            float percentage = elapsedTime / duration;
            renderers.frontRenderer.ForEach(it => it.color = new Color(1, 1, 1, percentage));
            renderers.backRenderer.color = new Color(1, 1, 1, 1 - percentage);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        renderers.frontRenderer.ForEach(it => it.color = new Color(1, 1, 1, 1));
        renderers.backRenderer.color = new Color(1, 1, 1, 0);
        currentCrossFade = null;
    }
}
