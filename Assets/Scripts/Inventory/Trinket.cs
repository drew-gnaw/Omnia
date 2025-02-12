using UnityEngine;
using UnityEngine.Serialization;

namespace Inventory {
    [CreateAssetMenu(fileName = "NewTrinket", menuName = "Inventory/Trinket")]
    public class Trinket : ScriptableObject {
        [SerializeField] public Sprite icon;
        [SerializeField] public string trinketName;
        [SerializeField] public string description;
    }
}
