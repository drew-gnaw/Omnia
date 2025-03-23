using UI;
using UnityEngine;

namespace Players.Buff {
    public class WrenchBuff : Buff {
        [SerializeField] private float atkBoost;

        public override void ApplyBuff() {
            player.weapons[player.selectedWeapon].damage += atkBoost;
        }

        public override void RevokeBuff() {
            player.weapons[player.selectedWeapon].damage -= atkBoost;
        }
    }
}
