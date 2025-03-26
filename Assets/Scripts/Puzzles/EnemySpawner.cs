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

    private readonly List<Enemy> ownedEnemies = new();
    private CountdownTimer spawnTimer;

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

    private void TrySpawn() {
        if (ownedEnemies.Count >= maxSpawns) return;

        var instance = Instantiate(spawn, spawnPoint.position, Quaternion.identity).GetComponent<Enemy>();
        ownedEnemies.Add(instance);
        Instantiate(explosion, spawnPoint.position, Quaternion.identity);
    }

    private void HandleEnemyDeath(Enemy enemy) {
        ownedEnemies.Remove(enemy);
    }
}

