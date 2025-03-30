using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Players.Buff {
    public class PickaxeBuff : Buff {
        [SerializeField] private float critMultiplierBuff;

        public override void ApplyBuff() {
            player.critMultiplier += critMultiplierBuff;
        }

        public override void RevokeBuff() {
            player.critMultiplier -= critMultiplierBuff;
        }
    }
}
