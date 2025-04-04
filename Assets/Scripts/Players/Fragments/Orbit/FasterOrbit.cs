using System.Collections.Generic;
using UnityEngine;

namespace Players.Fragments {
    public class FasterOrbit : Fragment {
        [SerializeField] private float increase;

		private OrbitObject orbitObject;
        public override void ApplyBuff() {
            base.ApplyBuff();
            orbitObject = player.GetComponentInChildren<OrbitObject>();
            if (orbitObject) {
                orbitObject.orbitSpeed += increase;
            }
        }

        public override void RevokeBuff() {
            orbitObject = player.GetComponentInChildren<OrbitObject>();
            if (orbitObject) {
                orbitObject.orbitSpeed -= increase;
            }
        }
    }
}
