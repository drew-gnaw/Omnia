using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class UIController : MonoBehaviour {
        private static UIController Instance { get; set; }

        [SerializeField] internal GameObject[] prefabs;
        [SerializeField] internal Canvas canvas;

        private Dictionary<EnemyBase, EnemyUI> enemies;

        public void Awake() {
            Instance = Instance ? Instance : this;
            if (Instance != this) Destroy(this);

            EnemyBase.Spawn += OnEnemySpawn;
            EnemyBase.Death += OnEnemyDeath;
            enemies = new Dictionary<EnemyBase, EnemyUI>();
        }

        public void Update() {
            foreach (var (enemy, ui) in enemies) ui.Move(WorldToUIPoint(enemy.transform.position, canvas.worldCamera));
        }

        private void OnEnemySpawn(EnemyBase enemy) {
            var ui = prefabs[0];
            enemies[enemy] = Instantiate(ui, canvas.transform).GetComponent<EnemyUI>().Of(enemy);
        }

        private void OnEnemyDeath(EnemyBase enemy) {
            if (enemies.Remove(enemy, out var ui)) Destroy(ui.gameObject);
        }

        private static Vector2 WorldToUIPoint(Vector2 position, Camera camera) {
            var p = camera.WorldToViewportPoint(position);
            return new Vector2(camera.scaledPixelWidth * (p.x * 2 - 1) / 2, camera.scaledPixelHeight * (p.y * 2 - 1) / 2);
        }
    }
}
