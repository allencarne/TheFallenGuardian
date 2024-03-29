using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    [Header("Inventory")]
    public Transform itemsParent;

    InventorySlot[] iSlots;

    private void Start()
    {
        inventory.onItemChangedCallback += UpdateUI;

        iSlots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void UpdateUI()
    {
        // Loop through each inventory slot
        for (int i = 0; i < iSlots.Length; i++)
        {
            // Check if the current slot index is within the bounds of the inventory items list
            if (i < inventory.items.Count)
            {
                // If there is an item at the current index in the inventory, add it to the corresponding slot in the UI
                iSlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                // If there is no item at the current index in the inventory, clear the corresponding slot in the UI
                iSlots[i].ClearSlot();
            }
        }
    }
}
