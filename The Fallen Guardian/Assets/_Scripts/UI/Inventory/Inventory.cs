using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        if (items.ContainsKey(item))
            items[item]++;
        else
            items.Add(item, 1);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        return true; // Return true to indicate the item was successfully added
    }

    public void Remove(Item item)
    {
        if (items[item] > 1)
            items[item]--;
        else
            items.Remove(item);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
