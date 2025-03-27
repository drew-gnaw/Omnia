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
    private TankState State { get; set; } = TankState.Inactive;
    public static event TankDelegate? TankDeactivated;
    public static event TankDelegate? TankActivated;
    public delegate void TankDelegate(Tank thank);

    private void Awake() {
        progressLever = lever.Value;
    }

    private void OnEnable() {
        progressLever.ProgressEvent += HandleShutoff;
    }

    private void OnDisable() {
        progressLever.ProgressEvent -= HandleShutoff;
    }

    public void Activate() {
        State = TankState.Active;
        PerformTransition(State);
        TankActivated?.Invoke(this);
    }

    public bool IsInactive() => State == TankState.Inactive;

    private void HandleShutoff(IProgress progress) {
        if (progress != null && Mathf.Approximately(progress.Progress, 1f) && State == TankState.Active) {
            State = TankState.Inactive;
            PerformTransition(State);
            TankDeactivated?.Invoke(this);
        }
    }

    private void PerformTransition(TankState state) {
        StartCoroutine(CrossFade(GetSpriteForState(state)));
    }

    private Sprite GetSpriteForState(TankState state) {
        return state switch {
            TankState.Active => crackedTank,
            TankState.Inactive => crackedDeactivatedTank,
            TankState.Broken => brokenTank,
            _ => normalTank,
        };
    }

    private IEnumerator CrossFade(Sprite sprite) {
        yield return null;
    }
}
