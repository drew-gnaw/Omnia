using Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory {
    [CreateAssetMenu(fileName = "Wrench", menuName = "Inventory/Trinket/Wrench")]
    public class Wrench : Trinket {
        [SerializeField] private float damageBuff = 2f;

        public override void ApplyEffect(Player player) {
            if (player != null) {
                player.weapons[player.selectedWeapon].damage += damageBuff;
            }
        }

        public override void RemoveEffect(Player player) {
            if (player != null) {
                player.weapons[player.selectedWeapon].damage -= damageBuff;
            }
        }
    }
}
