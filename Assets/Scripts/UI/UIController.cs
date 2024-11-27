using System.Collections.Generic;
using Cinemachine;
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

            CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdate);
            Enemy.Death += OnEnemyDeath;
            Enemy.Spawn += OnEnemySpawn;

            enemies = new Dictionary<Enemy, EnemyUI>();
        }

        private void OnCameraUpdate(CinemachineBrain it) {
            foreach (var ui in enemies.Values) ui.OnCameraUpdate();
        }

        private void OnEnemyDeath(Enemy enemy) {
            if (enemies.Remove(enemy, out var ui)) Destroy(ui.gameObject);
        }

        private void OnEnemySpawn(Enemy enemy) {
            var ui = prefabs[0];
            enemies[enemy] = Instantiate(ui, transform).GetComponent<EnemyUI>()!.Of(enemy);
        }
    }
}
