using Enemies;
using Enemies.Crab;
using Enemies.Crab.Animation;
using Puzzle;
using System.Collections;
using UnityEngine;
using Utils;

public class OpacityModifier : MonoBehaviour {

    [SerializeField] private Crab crab;
    [SerializeField] private InterfaceReference<IProgress> progressObject;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float fadeDuration = 0.5f;
#nullable enable
    private Coroutine? currentCoroutine;
    private IProgress Progress => progressObject.Value;

    private void OnEnable() {
        Enemy.Spawn += HandleSpawn;
        Progress.ProgressEvent += HandleProgress;
    }

    private void OnDisable() {
        Enemy.Spawn -= HandleSpawn;
        Progress.ProgressEvent -= HandleProgress;
        crab.animationStateMachine.OnStateChangedEvent -= HandleStateChange;
    }

    private void HandleSpawn(Enemy e) {
        if (e != crab) return;
        crab.animationStateMachine.OnStateChangedEvent += HandleStateChange;
    }

    private void HandleProgress(IProgress progress) {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, progress.Progress);
    }

    private void HandleStateChange(IState prev, IState next) {
        if (next is PopOutAnimation) {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(FadeSprite(spriteRenderer.color.a, 1f, fadeDuration));
        } else if (next is HideAnimation) {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(FadeSprite(spriteRenderer.color.a, 0f, fadeDuration));
        }
    }

    private IEnumerator FadeSprite(float startAlpha, float endAlpha, float duration) {
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Use an ease-out function to slow the fade at the end
            float easedT = 1f - Mathf.Pow(1f - t, 3f); // Cubic ease-out

            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, easedT);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);

            yield return null;
        }
    }
}
