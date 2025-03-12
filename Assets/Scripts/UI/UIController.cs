using System.Collections.Generic;
using Cinemachine;
using Enemies;
using UnityEngine;

namespace UI {
    public class UIController : MonoBehaviour {
        public static UIController Instance { get; set; }
 
        /**
            0: EnemyUI, 
            1: PlayerHealthBar,
            2: PlayerFlowBar,
            3: DamageMarker,
         */
        private enum UiPrefabs {
            EnemyUI = 0,
            PlayerHealthBar = 1,
            PlayerFlowBar = 2,
            DamageMarker = 3
        }
        [SerializeField] internal GameObject[] prefabs;
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
            Enemy.Damage += OnEnemyDamage;
            Enemy.Death += OnEnemyDeath;
            CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdate);

            InitializePlayerUI();
        }

        public void OnDisable() {
            Enemy.Spawn -= OnEnemySpawn;
            Enemy.Damage -= OnEnemyDamage;
            Enemy.Death -= OnEnemyDeath;
            CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdate);
        }

        private void OnCameraUpdate(CinemachineBrain it) {
            foreach (var ui in enemies.Values) ui.OnCameraUpdate();
        }

        private void OnEnemyDeath(Enemy enemy) {
            if (enemies.Remove(enemy, out var ui)) Destroy(ui.gameObject);
        }

        private void OnEnemyDamage(Enemy enemy, float damage) {
            var marker = Instantiate(prefabs[(int) UiPrefabs.DamageMarker]).GetComponent<DamageMarker>();
            marker.Initialize(enemy.transform.position, damage);
        }

        private void OnEnemySpawn(Enemy enemy) {
            var ui = prefabs[(int) UiPrefabs.EnemyUI];
            var prefab = Instantiate(ui, canvas.transform).GetComponent<EnemyUI>()!.Of(enemy);
            enemies[enemy] = prefab;
        }

        private void InitializePlayerUI() {
            var healthBar = Instantiate(prefabs[(int) UiPrefabs.PlayerHealthBar], canvas.transform);
            var flowBar = Instantiate(prefabs[(int) UiPrefabs.PlayerFlowBar], canvas.transform);

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
