using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int inventorySlots;
    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        // Check if there's enough room in the inventory for the new item
        if (items.Count >= inventorySlots)
        {
            Debug.Log("Not enough room.");
            return false; // Return false if there's no room
        }

        // Add the item to the inventory list
        items.Add(item);

        // If there's a callback registered for when items change, invoke it
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        return true; // Return true to indicate the item was successfully added
    }

    public void Remove(Item item)
    {
        // Remove the specified item from the inventory list
        items.Remove(item);

        // If there's a callback registered for when items change, invoke it
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
