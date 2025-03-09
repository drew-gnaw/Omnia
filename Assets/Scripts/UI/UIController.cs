using System.Collections.Generic;
using Cinemachine;
using Enemies;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI {
    public class UIController : MonoBehaviour {
        public static UIController Instance { get; set; }

        [SerializeField] internal GameObject[] prefabs; // 0: EnemyUI, 1: PlayerHealthBar, 2: PlayerFlowBar
        [SerializeField] internal Canvas canvas;
        // Used for setting the color of the player's health bar (likely temporary)
        [SerializeField] internal Color defaultHealthColor = Color.red;
        [SerializeField] internal Color buffHealthColor = Color.yellow;

        private Dictionary<Enemy, EnemyUI> enemies;
        private PlayerUI playerUI;

        public void Awake() {
            Instance = Instance != null ? Instance : this;
            if (Instance != this) Destroy(gameObject);

            enemies = new Dictionary<Enemy, EnemyUI>();
        }

        public void OnEnable() {
            Enemy.Spawn += OnEnemySpawn;
            Enemy.Death += OnEnemyDeath;
            CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdate);

            InitializePlayerUI();
        }

        public void OnDisable() {
            Enemy.Spawn -= OnEnemySpawn;
            Enemy.Death -= OnEnemyDeath;
            CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdate);
        }

        private void OnCameraUpdate(CinemachineBrain it) {
            foreach (var ui in enemies.Values) ui.OnCameraUpdate();
        }

        private void OnEnemyDeath(Enemy enemy) {
            if (enemies.Remove(enemy, out var ui)) Destroy(ui.gameObject);
        }

        private void OnEnemySpawn(Enemy enemy) {
            var ui = prefabs[0];
            var prefab = Instantiate(ui, canvas.transform).GetComponent<EnemyUI>()!.Of(enemy);
            enemies[enemy] = prefab;
        }

        private void InitializePlayerUI() {
            var healthBar = Instantiate(prefabs[1], canvas.transform);
            var flowBar = Instantiate(prefabs[2], canvas.transform);

            playerUI = new PlayerUI(healthBar.GetComponent<HealthBar>(), flowBar.GetComponent<FlowBar>());
        }

        public void UpdatePlayerHealth(float current, float max) {
            playerUI.HealthBar.UpdateBar(current, max);
        }

        public void UpdatePlayerFlow(float current, float max) {
            playerUI.FlowBar.UpdateBar(current, max);
        }

        public void SetHealthBarColor(Color c) {
            playerUI.HealthBar.SetColor(c);
        }
    }

    public class PlayerUI {
        public HealthBar HealthBar { get; }
        public FlowBar FlowBar { get; }

        public PlayerUI(HealthBar healthBar, FlowBar flowBar) {
            HealthBar = healthBar;
            FlowBar = flowBar;
        }
    }
}
