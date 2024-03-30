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
        // Subscribe to the inventory's item changed event
        inventory.onItemChangedCallback += UpdateUI;

        // Get all inventory slots from the itemsParent
        iSlots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void UpdateUI()
    {
        // Loop through all inventory slots
        for (var i = 0; i < iSlots.Length; i++)
        {
            // If there are items in the inventory to display
            if (i < inventory.items.Count)
            {
                // Add the item to the corresponding slot
                iSlots[i].AddItem(inventory.items.ElementAt(i).Key);
                // Display the quantity of the item in the slot
                iSlots[i].amount.text = inventory.items.ElementAt(i).Value.ToString();
            }
            // If there are no items to display in the slot
            else
            {
                // Clear the slot
                iSlots[i].ClearSlot();
                // Clear the quantity display
                iSlots[i].amount.text = "";
            }
        }
    }
}
