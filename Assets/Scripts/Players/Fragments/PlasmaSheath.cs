using System.Collections.Generic;
using UnityEngine;

namespace Players.Fragments {
    public class PlasmaSheath : Fragment {
        [SerializeField] private GameObject sheathPrefab;

        private GameObject sheathObject;
        public override void ApplyBuff() {
            base.ApplyBuff();
            sheathObject = Instantiate(sheathPrefab, player.transform.position, Quaternion.identity);
            sheathObject.transform.SetParent(player.transform);
        }

        public override void RevokeBuff() {
            Destroy(sheathObject);
        }
    }
}
