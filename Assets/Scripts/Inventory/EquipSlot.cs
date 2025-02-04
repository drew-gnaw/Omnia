using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    private TrinketSlot equippedTrinket;

    public bool HasTrinket() => equippedTrinket != null;

    public void Equip(TrinketSlot trinketSlot)
    {
        equippedTrinket = trinketSlot;
        icon.sprite = trinketSlot.GetComponent<Image>().sprite;
        icon.enabled = true;
    }

    public void Unequip()
    {
        if (equippedTrinket != null)
        {
            equippedTrinket.SetEquipped(false);
            equippedTrinket = null;
            icon.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Unequip();
    }
}
