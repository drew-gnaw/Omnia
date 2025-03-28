using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Players.Buff {
    public class GearBuff : Buff {
        [SerializeField] private float angleReduction;

        public override void ApplyBuff() {
            Shotgun shotgun = player.weapons.OfType<Shotgun>().FirstOrDefault();

            if (shotgun) {
                shotgun.blastAngle -= angleReduction;
            }
        }

        public override void RevokeBuff() {
            Shotgun shotgun = player.weapons.OfType<Shotgun>().FirstOrDefault();

            if (shotgun) {
                shotgun.blastAngle += angleReduction;
            }
        }
    }
}
