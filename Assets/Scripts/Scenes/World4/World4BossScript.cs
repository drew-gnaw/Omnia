using Enemies;
using Enemies.Bird;
using Enemies.Sundew;
using Puzzle;
using UnityEngine;

public class World4BossScript : MonoBehaviour {
    [SerializeField] private GameObject grassBlock;
    [SerializeField] private Sundew bossSundew;

    private void OnEnable() {
        Enemy.Death += HandleEnemyComplete;
    }

    private void OnDisable() {
        Enemy.Death -= HandleEnemyComplete;
    }

    private void HandleEnemyComplete(Enemy e) {
        if (e != bossSundew) return;
        Destroy(grassBlock);
    }
}
