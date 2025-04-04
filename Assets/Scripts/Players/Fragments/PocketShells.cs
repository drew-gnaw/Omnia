using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Players.Fragments {
    public class PocketShells : Fragment {
        [SerializeField] private int extraAmmo;
        public override void ApplyBuff() {
            base.ApplyBuff();
            Shotgun shotgun = player.weapons.OfType<Shotgun>().FirstOrDefault();

            if (shotgun != null) {
                shotgun.maxAmmoCount += extraAmmo;
            }
        }

        public override void RevokeBuff() {
            Shotgun shotgun = player.weapons.OfType<Shotgun>().FirstOrDefault();

            if (shotgun != null) {
                shotgun.maxAmmoCount -= extraAmmo;
            }
        }
    }
}
