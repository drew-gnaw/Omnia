using Enemies;
using UnityEngine;

namespace Players.Fragments {
    public class EchoQuiver : Fragment {
        [SerializeField] private GameObject purpleArrowPrefab;

        public void SpawnArrow(float damage, Enemy target) {
            if (target == null) return;

            Vector2 offset = Random.insideUnitCircle.normalized * 1f;
            Vector3 spawnPos = player.Center + (Vector3)offset;

            GameObject arrow = Instantiate(purpleArrowPrefab, spawnPos, Quaternion.identity);
            PurpleArrow arrowScript = arrow.GetComponent<PurpleArrow>();
            arrowScript.Initialize(target);
        }

        public override void ApplyBuff() {
            base.ApplyBuff();
            Player.OnEnemyHit += SpawnArrow;
        }

        public override void RevokeBuff() {
            Player.OnEnemyHit -= SpawnArrow;
        }
    }
}
