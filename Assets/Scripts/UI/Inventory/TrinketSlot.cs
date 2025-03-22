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
    }

    public void OnPointerEnter(PointerEventData eventData) {
        InventoryManager.Instance.UpdateDescription(trinket.trinketName, trinket.description);
    }

    public void OnPointerExit(PointerEventData eventData) {
        InventoryManager.Instance.ClearDescription();
    }

    public void OnPointerClick(PointerEventData eventData) {
        InventoryManager.Instance.EquipTrinket(this);
    }

    public void SetEquipped(bool equipped) {
        isEquipped = equipped;
        iconImage.color = equipped ? new Color(1, 1, 1, 0.2f) : Color.white; // Grey out if equipped

        if (equipped) {
            trinket.ApplyEffect(player);
        } else {
            trinket.RemoveEffect(player);
        }
    }
}
