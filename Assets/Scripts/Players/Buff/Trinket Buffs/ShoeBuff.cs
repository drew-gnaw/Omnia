using UI;
using UnityEngine;

namespace Players.Buff {
    public class ShoeBuff : Buff {
        public override void ApplyBuff() {
            player.shoeEquipped = true;
        }

        public override void RevokeBuff() {
            player.shoeEquipped = false;
        }
    }
}
