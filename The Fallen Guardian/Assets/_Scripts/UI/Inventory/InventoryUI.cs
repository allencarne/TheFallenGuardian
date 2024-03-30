using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    public Transform itemsParent;
    InventorySlot[] iSlots;

    private void Start()
    {
        // Get all inventory slots from the itemsParent
        iSlots = itemsParent.GetComponentsInChildren<InventorySlot>();

        // Assign slot indices to each slot
        for (int i = 0; i < iSlots.Length; i++)
        {
            iSlots[i].slotIndex = i; // Assign slot index based on position in array
        }

        // Subscribe to the inventory's item changed event
        inventory.onItemChangedCallback += UpdateUI;
    }

    void UpdateUI()
    {
        // Loop through all inventory slots
        for (int i = 0; i < iSlots.Length; i++)
        {
            // If there's an item in the corresponding slot
            if (i < inventory.items.Length && inventory.items[i] != null)
            {
                // Add the item to the slot
                iSlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                // Clear the slot if it's empty
                iSlots[i].ClearSlot();
            }
        }
    }
}
