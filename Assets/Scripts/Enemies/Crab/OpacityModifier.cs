using Puzzle;
using UnityEngine;

public class OpacityModifier : MonoBehaviour
{
    [SerializeField] private InterfaceReference<IProgress> progressObject;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private IProgress Progress => progressObject.Value;

    private void OnEnable() {
        Progress.ProgressEvent += HandleProgress;
    }

    private void OnDisable() {
        Progress.ProgressEvent -= HandleProgress;
    }

    private void HandleProgress(IProgress progress) {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, progress.Progress);
    }
}
