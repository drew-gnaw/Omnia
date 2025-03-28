using Enemies;
using Enemies.Spawnball;
using Omnia.Utils;
using Puzzle;
using System;
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
    [SerializeField] private float spawnCoolDown;
    [SerializeField] private Spawnball spawnball;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Transform spawnballSpawnPoint;
    [SerializeField] private List<TransformInfo> spawnLocations;
    private IProgress progressLever;
#nullable enable
    private CountdownTimer? spawnTimer;
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

    private void Update() {
        if (spawnTimer != null && spawnTimer.IsRunning) {
            spawnTimer.Tick(Time.deltaTime);
        } else if (GetUnoccupiedTransformInfos().Count > 0) {
            if (spawnTimer == null || !spawnTimer.IsRunning) {
                spawnTimer = new CountdownTimer(spawnCoolDown);
                spawnTimer.Start();
            }
        }

        if (spawnTimer != null && !spawnTimer.IsRunning && GetUnoccupiedTransformInfos().Count > 0) {
            TrySpawn();
        }
    }

    private void TrySpawn() {
        if (GetUnoccupiedTransformInfos().Count == 0) return;

        var instance = GetConfiguredSpawnball();
        Instantiate(explosion, spawnballSpawnPoint.position, Quaternion.identity);
    }

    public void Activate() {
        State = TankState.Active;
        PerformTransition(State);
        TankActivated?.Invoke(this);
    }

    public bool IsInactive() => State == TankState.Inactive;
    private GameObject GetConfiguredSpawnball() {
        Spawnball instance = Instantiate(spawnball, spawnballSpawnPoint.position, Quaternion.identity);
        
        TransformInfo info = GetRandomSpawnLocationInfo();

        instance.spawn = enemyPrefab; 
        instance.target = info.transform;

        instance.NotifyOnSpawn += HandleEnemySpawn;


        
        //
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
        if (inactiveSpawnLocations.Count == 0 ) {
            Debug.LogWarning($"Something Went terrible wrong for tank {gameObject.name} to try and spawn when all locations are occupied");
            return new TransformInfo(this.gameObject.transform, null);
        }
        return spawnLocations[UnityEngine.Random.Range(0, inactiveSpawnLocations.Count)];
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
