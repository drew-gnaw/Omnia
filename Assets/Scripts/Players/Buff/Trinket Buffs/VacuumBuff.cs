using System.Linq;
using UI;
using UnityEngine;

namespace Players.Buff {
    public class VacuumBuff : Buff {
        [SerializeField] private float speedDebuff;

        public override void ApplyBuff() {
            player.moveSpeed -= speedDebuff;
        }

        public override void RevokeBuff() {
            player.moveSpeed += speedDebuff;
        }
    }
}
