using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public int inventorySlots;
    public Dictionary<Item, int> items = new Dictionary<Item, int>();

    public bool Add(Item item)
    {
        // Check if there's enough room in the inventory for the new item
        if (items.Count >= inventorySlots)
        {
            Debug.Log("Not enough room.");
            return false; // Return false if there's no room
        }

        // If the item is already in the inventory, increase its quantity
        if (items.ContainsKey(item))
        {
            items[item]++;
        }
        // If it's a new item, add it to the inventory with a quantity of 1
        else
        {
            items.Add(item, 1);
        }

        // Invoke the callback to notify listeners that an item has been added
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        return true; // Return true to indicate the item was successfully added
    }

    public void Remove(Item item)
    {
        // If the item has more than one quantity, decrease its quantity
        if (items[item] > 1)
        {
            items[item]--;
        }
        // If the item has only one quantity, remove it from the inventory
        else
        {
            items.Remove(item);
        }

        // Invoke the callback to notify listeners that an item has been removed
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
