using Enemies.Spawnball;
using Puzzle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [Serializable]
    private class TransformInfo {
        public Transform transform;
        public bool isOccupied;
    }
    private enum TankState { Inactive, Active, Broken }
    [SerializeField] private Sprite crackedTank;
    [SerializeField] private Sprite normalTank;
    [SerializeField] private Sprite brokenTank;
    [SerializeField] private Sprite crackedDeactivatedTank;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private InterfaceReference<IProgress> lever;
    [SerializeField] private Spawnball spawnball;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<TransformInfo> spawnLocations;
    private IProgress progressLever;

#nullable enable
    private TankState State { get; set; } = TankState.Inactive;
    public static event TankDelegate? TankDeactivated;
    public static event TankDelegate? TankActivated;
    public delegate void TankDelegate(Tank thank);

    private void Awake() {
        progressLever = lever.Value;
        spawner.SetMaxSpawn(spawnLocations.Count);
    }

    private void Start() {
        spawner.GetSpawn = GetConfiguredSpawnball;


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
    private GameObject GetConfiguredSpawnball() {
        Spawnball instance = Instantiate(spawnball);

        instance.spawn = enemyPrefab; 
        instance.target = GetRandomSpawnLocation();

        return instance.gameObject;
    }

    private Transform GetRandomSpawnLocation() {
        var inactiveSpawnLocations = spawnLocations.FindAll(it => !it.isOccupied);
        if (inactiveSpawnLocations.Count == 0 ) {
            Debug.LogWarning($"Something Went terrible wrong for tank {gameObject.name} to try and spawn when all locations are occupied");
            return this.gameObject.transform;
        }

        TransformInfo info = spawnLocations[UnityEngine.Random.Range(0, inactiveSpawnLocations.Count)];
        info.isOccupied = true;

        return info.transform;
    }

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

    private void HandleSpawn() {

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
