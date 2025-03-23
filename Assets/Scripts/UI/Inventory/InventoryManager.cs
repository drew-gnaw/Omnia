using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Utils;

public class InventoryManager : PersistentSingleton<InventoryManager> {
    [SerializeField] private GameObject inventoryUI; // Reference to Inventory UI Panel
    [SerializeField] private TrinketSlot[] trinketSlots; // Grid of trinket slots
    [SerializeField] private TrinketSlot equippedTrinket;
    [SerializeField] private TextMeshProUGUI descriptionText; // Description UI text box

    public bool IsInventoryOpen { get; private set; }

    private bool isShaking = false;

    protected override void OnAwake() {
    }

    private void Start() {
        InitializeTrinketSlots();
        ClearDescription();
        inventoryUI.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            ToggleInventory();
        } else if (Input.GetKeyDown(KeyCode.Tab)) {
            CloseInventory();
        }
    }

    private void InitializeTrinketSlots() {
        foreach (var slot in trinketSlots) {
            slot.Initialize();
        }
    }

    public void ToggleInventory() {
        IsInventoryOpen = !inventoryUI.activeSelf;
        inventoryUI.SetActive(IsInventoryOpen);
    }

    public void CloseInventory() {
        IsInventoryOpen = false;
        inventoryUI.SetActive(false);
    }

    public void EquipTrinket(TrinketSlot selectedTrinket) {
        if (selectedTrinket.IsEquipped)
            return;

        equippedTrinket?.SetEquipped(false);
        equippedTrinket = selectedTrinket;
        selectedTrinket.SetEquipped(true);
    }


    public void UpdateDescription(string trinketName, string description) {
        descriptionText.text = trinketName + "\n" + description;
    }

    public void ClearDescription() {
        descriptionText.text = "";
    }
}
