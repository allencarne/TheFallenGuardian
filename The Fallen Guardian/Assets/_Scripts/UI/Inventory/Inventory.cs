using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int inventorySlots;
    public Item[] items; // Array to hold items

    private void Awake()
    {
        items = new Item[inventorySlots]; // Initialize the array with the specified number of slots
    }

    public bool Add(Item item)
    {
        // Check if there's enough room in the inventory for the new item
        if (Array.FindIndex(items, x => x == null) == -1)
        {
            Debug.Log("Not enough room.");
            return false; // Return false if there's no room
        }

        // Find the first empty slot in the inventory
        int emptySlotIndex = Array.FindIndex(items, x => x == null);
        if (emptySlotIndex != -1)
        {
            // Check if the item already exists in the inventory
            int existingItemIndex = Array.FindIndex(items, x => x == item);
            if (existingItemIndex != -1)
            {
                // If the item exists, increase its quantity
                items[existingItemIndex].quantity++;
            }
            else
            {
                // If the item is new, add it to the empty slot with a quantity of 1
                item.quantity = 1;
                items[emptySlotIndex] = item;
            }
        }
        else
        {
            Debug.Log("Inventory is full.");
            return false; // Return false if no empty slot is found
        }

        // Invoke the callback to notify listeners that an item has been added
        onItemChangedCallback?.Invoke();

        return true; // Return true to indicate the item was successfully added
    }

    public void Remove(Item item)
    {
        // Find the index of the item in the inventory
        int itemIndex = Array.IndexOf(items, item);
        if (itemIndex != -1)
        {
            // Remove the item from the inventory by setting its slot to null
            items[itemIndex] = null;
        }

        // Invoke the callback to notify listeners that an item has been removed
        onItemChangedCallback?.Invoke();
    }
}
