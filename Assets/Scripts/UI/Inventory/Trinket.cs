using System;
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

        private bool isLocked = true;
        private Buff activeBuffInstance;

        // Event that gets triggered when isLocked changes
        public event Action<Trinket> OnTrinketUnlocked;

        public bool IsLocked {
            get => isLocked;
            set {
                if (isLocked != value) { // Only trigger if it actually changes
                    isLocked = value;
                    if (!isLocked) {
                        OnTrinketUnlocked?.Invoke(this); // Notify all subscribers
                    }
                }
            }
        }

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
