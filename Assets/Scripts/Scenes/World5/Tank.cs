using Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    private enum TankState { Inactive, Active, Broken }
    [SerializeField] private Sprite crackedTank;
    [SerializeField] private Sprite normalTank;
    [SerializeField] private Sprite brokenTank;
    [SerializeField] private Sprite crackedDeactivatedTank;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private InterfaceReference<IProgress> lever;
    private IProgress progressLever;

#nullable enable
    private TankState state = TankState.Inactive;
    public event TankDelegate? TankDeactivated;
    public delegate void TankDelegate(Tank thank);

    private void Awake() {
        progressLever = lever.Value;
    }

    private void OnEnable() {
        progressLever.ProgressEvent += HandlePulled;
    }

    private void OnDisable() {
        progressLever.ProgressEvent -= HandlePulled;
    }

    private void HandlePulled(IProgress progress) {
        if (progress != null && progress.Progress == 1 && state == TankState.Active) {
            state = TankState.Inactive;
            PerformTransition(state);
            TankDeactivated?.Invoke(this);
        }
    }

    private void PerformTransition(TankState state) {
        Sprite activeSprite = state switch {
            TankState.Active => crackedTank,
            TankState.Inactive => crackedDeactivatedTank,
            TankState.Broken => brokenTank,
            _ => normalTank,
        };

        StartCoroutine(CrossFade(activeSprite));
    }

    private IEnumerator CrossFade(Sprite sprite) {
        yield return null;
    }
}
