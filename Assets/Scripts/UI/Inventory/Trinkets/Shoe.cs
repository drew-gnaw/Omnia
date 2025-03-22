using Players;
using Players.Buff;
using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "Shoe", menuName = "Inventory/Trinket/Shoe")]
    public class Shoe : Trinket {
        [SerializeField] private Buff buff;
        [SerializeField] private float speedIncrease = 2f;
        
        private Buff buffInstance;

        public override void ApplyEffect(Player player) {
            buffInstance = Instantiate(buff, player.buffsParent);
            buffInstance.Initialize(player);
            buffInstance.ApplyBuff();
        }

        public override void RemoveEffect(Player player) {
            if (player != null) {
                player.moveSpeed -= speedIncrease;
            }
        }
    }
}
