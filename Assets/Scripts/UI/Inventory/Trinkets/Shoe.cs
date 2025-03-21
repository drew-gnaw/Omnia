using Players;
using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "Shoe", menuName = "Inventory/Trinket/Shoe")]
    public class Shoe : Trinket {
        [SerializeField] private float speedIncrease = 2f;

        public override void ApplyEffect(Player player) {
            if (player != null) {
                player.moveSpeed += speedIncrease;
            }
        }

        public override void RemoveEffect(Player player) {
            if (player != null) {
                player.moveSpeed -= speedIncrease;
            }
        }
    }
}
