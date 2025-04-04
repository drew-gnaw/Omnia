using System.Collections.Generic;
using UnityEngine;

namespace Players.Fragments {
    public class ProtectiveOrbit : Fragment {
        [SerializeField] private int increase;

		private OrbitObject orbitObject;
        public override void ApplyBuff() {
            base.ApplyBuff();
            orbitObject = player.GetComponentInChildren<OrbitObject>();
            if (orbitObject) {
                orbitObject.orbitalCount += increase;
            }
        }

        public override void RevokeBuff() {
            orbitObject = player.GetComponentInChildren<OrbitObject>();
            if (orbitObject) {
                orbitObject.orbitalCount -= increase;
            }
        }
    }
}
