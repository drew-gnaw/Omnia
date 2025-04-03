using System.Collections.Generic;
using UnityEngine;

namespace Players.Fragments {
    public class Alacrity : Fragment {
        [SerializeField] private float speedBoost;
        public override void ApplyBuff() {
            base.ApplyBuff();
            player.moveSpeed += speedBoost;
        }

        public override void RevokeBuff() {
            player.moveSpeed -= speedBoost;
        }
    }
}
