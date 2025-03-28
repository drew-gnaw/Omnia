using System.Collections.Generic;
using UnityEngine;
using Omnia.Utils;
using Enemies;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxSpawns;
    [SerializeField] private float spawnCoolDown;
    [SerializeField] private GameObject spawn;
    [SerializeField] private GameObject explosion;
#nullable enable
    public bool IsActive { get; set; } = true;
    // Necessary to dynamically change the spawned object by providing a different getter
    public GetSpawnObject GetSpawn;
    public delegate GameObject GetSpawnObject();

    private readonly List<Enemy> ownedEnemies = new();
    private CountdownTimer? spawnTimer;
    private void Awake() {
        GetSpawn = () => Instantiate(spawn);
    }

    private void Start() {
        TrySpawn();
    }

    private void OnEnable() {
        Enemy.Death += HandleEnemyDeath;
    }

    private void OnDisable() {
        Enemy.Death -= HandleEnemyDeath;
    }

    private void Update() {
        if (!IsActive) return;

        if (spawnTimer != null && spawnTimer.IsRunning) {
            spawnTimer.Tick(Time.deltaTime);
        } else if (ownedEnemies.Count < maxSpawns) {
            if (spawnTimer == null || !spawnTimer.IsRunning) {
                spawnTimer = new CountdownTimer(spawnCoolDown);
                spawnTimer.Start();
            }
        }

        if (spawnTimer != null && !spawnTimer.IsRunning && ownedEnemies.Count < maxSpawns) {
            TrySpawn();
        }
    }

    public void SetMaxSpawn(int maxSpawn) {
        this.maxSpawns = maxSpawn;
    }

    private void TrySpawn() {
        if (!IsActive || ownedEnemies.Count >= maxSpawns) return;

        var instance = GetSpawn();
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = Quaternion.identity;
        ownedEnemies.Add(instance.GetComponent<Enemy>());
        Instantiate(explosion, spawnPoint.position, Quaternion.identity);
    }

    private void HandleEnemyDeath(Enemy enemy) {
        ownedEnemies.Remove(enemy);
    }
}

