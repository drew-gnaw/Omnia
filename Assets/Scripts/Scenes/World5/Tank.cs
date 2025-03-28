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
        spawner.GetOwnedEnemiesCount = () => spawnLocations.FindAll(it => it.occupiedEnemy != null).Count;
    }

    private void OnEnable() {
        progressLever.ProgressEvent += HandleShutoff;
    }

    private void OnDisable() {
        progressLever.ProgressEvent -= HandleShutoff;
    }

    public void Activate() {
        ScreenShakeManager.Instance.Shake();
        PerformTransition(TankState.Active);
        spawner.IsActive = true;
        spawner.TrySpawn();
        TankActivated?.Invoke(this);
    }

    public void Break() {
        ScreenShakeManager.Instance.Shake(3, 1f);
        PerformTransition(TankState.Broken);
        spawner.IsActive = false;
        KillAllChildren();
    }

    public bool IsInactive() => State == TankState.Inactive;

    private void KillAllChildren() {
        spawner.KillAllChildren();
        spawnLocations.ForEach(it => it.occupiedEnemy?.Die());
    }

    // Spawnball factory for EnemySpawner that then uses events to modify Tank state
    private GameObject GetConfiguredSpawnball() {
        Spawnball instance = Instantiate(spawnball);
        TransformInfo info = GetRandomSpawnLocationInfo();

        instance.spawn = enemyPrefab;
        instance.target = info.transform;
        instance.NotifyOnSpawn += HandleEnemySpawn;

        //First register callbacks on spawnball, then on its spawned enemy 
        void HandleEnemySpawn(Enemy enemy) {
            info.occupiedEnemy = enemy;

            instance.NotifyOnSpawn -= HandleEnemySpawn;
            enemy.OnDeath += HandleEnemyDeath;

            void HandleEnemyDeath(Enemy e) {
                info.occupiedEnemy = null;
                enemy.OnDeath -= HandleEnemyDeath;
            }
        }

        return instance.gameObject;
    }

    private List<TransformInfo> GetUnoccupiedTransformInfos() {
        var res = spawnLocations.FindAll(it => it.occupiedEnemy == null);
        return res;
    }

    private TransformInfo GetRandomSpawnLocationInfo() {
        List<TransformInfo> inactiveSpawnLocations = GetUnoccupiedTransformInfos();
        if (inactiveSpawnLocations.Count == 0) {
            Debug.LogWarning($"Something Went terrible wrong for tank {gameObject.name} to try and spawn when all locations are occupied");
            return new TransformInfo(this.gameObject.transform, null);
        }
        return inactiveSpawnLocations[UnityEngine.Random.Range(0, inactiveSpawnLocations.Count)];
    }

    private void HandleShutoff(IProgress progress) {
        if (progress != null && Mathf.Approximately(progress.Progress, 1f) && State == TankState.Active) {
            PerformTransition(TankState.Inactive);
            spawner.IsActive = false;
            TankDeactivated?.Invoke(this);
        }
    }

    private void PerformTransition(TankState state) {
        State = state;
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
        spriteRenderer.sprite = sprite;
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
