using System.Linq;
using UI;
using UnityEngine;

namespace Players.Buff {
    public class GlockBuff : Buff {
        [SerializeField] private float critChanceBuff;

        public override void ApplyBuff() {
            player.critChance += critChanceBuff;
        }

        public override void RevokeBuff() {
            player.critChance -= critChanceBuff;
        }
    }
}
