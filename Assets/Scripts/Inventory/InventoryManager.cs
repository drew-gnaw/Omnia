using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI; // Reference to Inventory UI Panel
    [SerializeField] private TrinketSlot[] trinketSlots; // Grid of trinket slots
    [SerializeField] private EquipSlot[] equipSlots; // Equip slots
    [SerializeField] private TextMeshProUGUI descriptionText; // Description UI text box

    public static InventoryManager Instance { get; private set; }
    public bool IsInventoryOpen { get; private set; }

    private bool isShaking = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void Start()
    {
        InitializeTrinketSlots();
        inventoryUI.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    private void InitializeTrinketSlots()
    {
        foreach (var slot in trinketSlots)
        {
            slot.Initialize();
        }

        foreach (var slot in equipSlots)
        {
            slot.Initialize();
        }
    }

    public void ToggleInventory()
    {
        IsInventoryOpen = !inventoryUI.activeSelf;
        inventoryUI.SetActive(IsInventoryOpen);
    }

    public void EquipTrinket(TrinketSlot selectedTrinket)
    {
        if (selectedTrinket.IsEquipped)
            return;

        foreach (EquipSlot slot in equipSlots)
        {
            if (!slot.HasTrinket())
            {
                slot.Equip(selectedTrinket);
                selectedTrinket.SetEquipped(true);
                return;
            }
        }

        if (!isShaking) {
            StartCoroutine(ShakeSlot(equipSlots[0].gameObject));
        }
    }

    public void UnequipTrinket(EquipSlot slot)
    {
        slot.Unequip();
    }

    private IEnumerator ShakeSlot(GameObject slot) {
        isShaking = true;
        Vector3 originalPos = slot.transform.position;
        float duration = 0.2f;
        float magnitude = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float offset = Mathf.Sin(elapsed * 50) * magnitude;
            slot.transform.position = originalPos + new Vector3(offset, 0, 0);
            yield return null;
        }

        slot.transform.position = originalPos;
        isShaking = false;
    }

    public void UpdateDescription(string name, string description)
    {
        descriptionText.text = name + "\n" + description;
    }

    public void ClearDescription()
    {
        descriptionText.text = "";
    }
}
