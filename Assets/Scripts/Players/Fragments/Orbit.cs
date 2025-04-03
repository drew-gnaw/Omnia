using System.Collections.Generic;
using UnityEngine;

namespace Players.Fragments {
    public class Orbit : Fragment {
        [SerializeField] private GameObject orbitPrefab;

        private GameObject orbitObject;
        public override void ApplyBuff() {
            base.ApplyBuff();
            orbitObject = Instantiate(orbitPrefab, player.transform.position, Quaternion.identity);
            orbitObject.transform.SetParent(player.transform);
        }

        public override void RevokeBuff() {
            Destroy(orbitObject);
        }
    }
}
