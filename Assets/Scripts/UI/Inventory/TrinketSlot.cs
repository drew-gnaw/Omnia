using Inventory;
using Players;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrinketSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    [SerializeField] private Trinket trinket;
    private bool isEquipped = false;
    private Image iconImage;
    private Player player;

    public bool IsEquipped => isEquipped;
    public Image IconImage => iconImage;

    public void Initialize() {
        iconImage = GetComponent<Image>();
        iconImage.sprite = trinket.icon;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // Handle locked trinkets
        if (trinket.isLocked) {
            iconImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Greyed out
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!trinket.isLocked) {
            InventoryManager.Instance.UpdateDescription(trinket.trinketName, trinket.description);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        InventoryManager.Instance.ClearDescription();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (trinket.isLocked) return; // Prevent interaction if locked

        InventoryManager.Instance.EquipTrinket(this);
    }

    public void SetEquipped(bool equipped) {
        if (trinket.isLocked) return; // Do nothing if locked

        isEquipped = equipped;

        if (equipped) {
            iconImage.color = new Color(1f, 0.84f, 0f, 1f); // Gold color for equipped
            trinket.ApplyEffect(player);
        } else {
            iconImage.color = Color.white;
            trinket.RemoveEffect(player);
        }
    }
}
