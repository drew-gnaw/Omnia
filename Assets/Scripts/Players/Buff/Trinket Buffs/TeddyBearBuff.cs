using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Players.Buff {
    public class TeddyBearBuff : Buff {
        public static float cooldownTime = 10f;
        public override void ApplyBuff() {
            player.bearEquipped = true;
        }

        public override void RevokeBuff() {
            player.bearEquipped = false;
        }
    }
}
