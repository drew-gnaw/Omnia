using Inventory;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrinketSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Trinket trinket;
    private bool isEquipped = false;
    private Image iconImage; // Store reference to Image component

    public bool IsEquipped => isEquipped;

    public void Initialize()
    {
        iconImage = GetComponent<Image>();
        iconImage.sprite = trinket.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.UpdateDescription(trinket.trinketName, trinket.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.ClearDescription();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        InventoryManager.Instance.EquipTrinket(this);
    }

    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        iconImage.color = equipped ? new Color(1, 1, 1, 0.5f) : Color.white; // Grey out if equipped
    }
}
