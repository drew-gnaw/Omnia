using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Players.Buff {
    public class BandanaBuff : Buff {
        [SerializeField] private int healthBoost;

        public override void ApplyBuff() {
            player.maximumHealth += healthBoost;
            player.CurrentHealth += healthBoost;
            player.HealthBoosted = true;
        }

        public override void RevokeBuff() {
            player.maximumHealth -= healthBoost;
            player.CurrentHealth = Mathf.Max(1, player.CurrentHealth - healthBoost);
            player.HealthBoosted = false;
        }
    }
}
