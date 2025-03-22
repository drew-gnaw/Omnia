using UI;
using UnityEngine;

namespace Players.Buff {
    public class ShoeBuff : Buff {
        [SerializeField] private float speedBoost;

        public override void ApplyBuff() {
            player.moveSpeed += speedBoost;
        }

        public override void RevokeBuff() {
            player.moveSpeed += speedBoost;
        }
    }
}
