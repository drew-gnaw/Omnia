using Players;
using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "NewTrinket", menuName = "Inventory/Trinket")]
    public abstract class Trinket : ScriptableObject {
        [SerializeField] public Sprite icon;
        [SerializeField] public string trinketName;
        [SerializeField] public string description;

        public abstract void ApplyEffect(Player player);
        public abstract void RemoveEffect(Player player);
    }
}
