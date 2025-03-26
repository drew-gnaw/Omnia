using Enemies;
using Enemies.Bird;
using Puzzle;
using UnityEngine;

public class World3BossScript : MonoBehaviour
{
    [SerializeField] private GameObject steamGate;

    private void OnEnable() {
        Enemy.Death += HandleEnemyComplete;
    }

    private void OnDisable() {
        Enemy.Death -= HandleEnemyComplete;
    }

    private void HandleEnemyComplete(Enemy e) {
        if (e is not Bird) return;
        Destroy(steamGate);
    }
}
