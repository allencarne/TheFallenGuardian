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
        for (int i = 0; i < iSlots.Length; i++)
        {
            // If there are items in the inventory to display
            if (i < inventory.items.Count)
            {
                // Add the item to the corresponding slot
                iSlots[i].AddItem(inventory.items.ElementAt(i).Key);

                // Get the quantity of the item in the slot
                int stackSize = inventory.items.ElementAt(i).Value;

                // Check if stack size is greater than 1
                if (stackSize > 1)
                {
                    // Display the quantity of the item in the slot
                    iSlots[i].amount.text = stackSize.ToString();
                }
                else
                {
                    // Hide the quantity display
                    iSlots[i].amount.text = "";
                }
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
