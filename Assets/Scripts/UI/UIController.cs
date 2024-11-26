using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace UI {
    public class UIController : MonoBehaviour {
        private static UIController Instance { get; set; }

        [SerializeField] internal GameObject[] prefabs;
        [SerializeField] internal Canvas canvas;

        private Dictionary<Enemy, EnemyUI> enemies;

        public void Awake() {
            Instance = Instance != null ? Instance : this;
            if (Instance != this) Destroy(gameObject);

            Enemy.Spawn += OnEnemySpawn;
            Enemy.Death += OnEnemyDeath;
            enemies = new Dictionary<Enemy, EnemyUI>();
        }

        public void Update() {
            foreach (var (enemy, ui) in enemies) ui.Move(WorldToUIPoint(enemy.transform.position, canvas.worldCamera));
        }

        private void OnEnemySpawn(Enemy enemy) {
            var ui = prefabs[0];
            enemies[enemy] = Instantiate(ui, transform).GetComponent<EnemyUI>()!.Of(enemy);
        }

        private void OnEnemyDeath(Enemy enemy) {
            if (enemies.Remove(enemy, out var ui)) Destroy(ui.gameObject);
        }

        private static Vector2 WorldToUIPoint(Vector2 position, Camera camera) {
            var p = camera.WorldToViewportPoint(position);
            return new Vector2(camera.scaledPixelWidth * (p.x * 2 - 1) / 2, camera.scaledPixelHeight * (p.y * 2 - 1) / 2);
        }
    }
}
