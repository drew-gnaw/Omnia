using System.Linq;
using UI;
using UnityEngine;

namespace Players.Buff {
    public class MusicBoxBuff : Buff {
        public override void ApplyBuff() {
            player.musicBoxEquipped = true;
        }

        public override void RevokeBuff() {
            player.musicBoxEquipped = false;
        }
    }
}
