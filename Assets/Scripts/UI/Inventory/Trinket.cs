using Players;
using Players.Buff;
using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "New Trinket", menuName = "Inventory/Trinket")]
    public class Trinket : ScriptableObject {
        [SerializeField] public Sprite icon;
        [SerializeField] public string trinketName;
        [SerializeField] public string description;

        [SerializeField] private Buff buffPrefab;

        public bool isLocked = true;

        private Buff activeBuffInstance;

        public void ApplyEffect(Player player) {
            activeBuffInstance = BuffManager.Instance.ApplyBuff(buffPrefab);
        }

        public void RemoveEffect(Player player) {
            if (activeBuffInstance != null) {
                BuffManager.Instance.RemoveBuff(activeBuffInstance);
                activeBuffInstance = null;
            }
        }
    }
}
