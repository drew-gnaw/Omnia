using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Players.Fragments {
    public class PocketHarpoons : Fragment {
        [SerializeField] private int extraAmmo;
        public override void ApplyBuff() {
            base.ApplyBuff();
            HarpoonGun harpoonGun = player.weapons.OfType<HarpoonGun>().FirstOrDefault();

            if (harpoonGun != null) {
                harpoonGun.maxAmmoCount += extraAmmo;
            }
        }

        public override void RevokeBuff() {
            HarpoonGun harpoonGun = player.weapons.OfType<HarpoonGun>().FirstOrDefault();

            if (harpoonGun != null) {
                harpoonGun.maxAmmoCount -= extraAmmo;
            }
        }
    }
}
