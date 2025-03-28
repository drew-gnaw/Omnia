using Enemies;
using Enemies.Spawnball;
using Puzzle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
    private enum TankState { Inactive, Active, Broken }
    [SerializeField] private Sprite crackedTank;
    [SerializeField] private Sprite normalTank;
    [SerializeField] private Sprite brokenTank;
    [SerializeField] private Sprite crackedDeactivatedTank;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private InterfaceReference<IProgress> lever;
    [SerializeField] private Spawnball spawnball;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Enemy enemyPrefab;
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
        spawner.IsActive = false;
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
        spawner.IsActive = true;
        TankActivated?.Invoke(this);
    }

    public bool IsInactive() => State == TankState.Inactive;

    // Spawnball factory for EnemySpawner that then uses events to modify Tank state
    private GameObject GetConfiguredSpawnball() {
        Spawnball instance = Instantiate(spawnball);

        TransformInfo info = GetRandomSpawnLocationInfo();

        instance.spawn = enemyPrefab;
        instance.target = info.transform;

        instance.NotifyOnSpawn += HandleEnemySpawn;

        void HandleEnemySpawn(Enemy enemy) {
            instance.NotifyOnSpawn -= HandleEnemySpawn;
            info.occupiedEnemy = enemy;

            // Oh my god I'm gonna cry
            enemy.OnDeath += HandleEnemyDeath;
            void HandleEnemyDeath(Enemy e) {
                enemy.OnDeath -= HandleEnemyDeath;
                info.occupiedEnemy = null;
            }
        }

        return instance.gameObject;
    }
    private List<TransformInfo> GetUnoccupiedTransformInfos() {
        return spawnLocations.FindAll(it => it.occupiedEnemy == null);
    }

    private TransformInfo GetRandomSpawnLocationInfo() {
        List<TransformInfo> inactiveSpawnLocations = GetUnoccupiedTransformInfos();
        if (inactiveSpawnLocations.Count == 0) {
            Debug.LogWarning($"Something Went terrible wrong for tank {gameObject.name} to try and spawn when all locations are occupied");
            return new TransformInfo(this.gameObject.transform, null);
        }
        return spawnLocations[UnityEngine.Random.Range(0, inactiveSpawnLocations.Count)];
    }

    private void HandleShutoff(IProgress progress) {
        if (progress != null && Mathf.Approximately(progress.Progress, 1f) && State == TankState.Active) {
            State = TankState.Inactive;
            PerformTransition(State);
            spawner.IsActive = false;
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

    [Serializable]
    private class TransformInfo {
        public Transform transform;
        public Enemy? occupiedEnemy;

        public TransformInfo(Transform transform, Enemy? enemy) {
            this.transform = transform;
            this.occupiedEnemy = enemy;
        }
    }
}
