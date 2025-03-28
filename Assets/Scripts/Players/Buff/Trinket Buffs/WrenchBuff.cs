using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Players.Buff {
    public class WrenchBuff : Buff {
        [SerializeField] private float damageBoost;

        public override void ApplyBuff() {
            HarpoonGun harpoongun = player.weapons.OfType<HarpoonGun>().FirstOrDefault();

            if (harpoongun) {
                harpoongun.damage += damageBoost;
            }
        }

        public override void RevokeBuff() {
            HarpoonGun harpoongun = player.weapons.OfType<HarpoonGun>().FirstOrDefault();

            if (harpoongun) {
                harpoongun.damage -= damageBoost;
            }
        }
    }
}
