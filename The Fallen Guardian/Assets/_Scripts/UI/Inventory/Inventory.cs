using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int inventorySlots;
    public List<Item> items = new List<Item>();

    public UnityEvent OnItemChanged;

    public bool Add(Item item)
    {
        if (items.Count >= inventorySlots)
        {
            Debug.Log("Not enough room.");
            return false;
        }

        items.Add(item);

        OnItemChanged.Invoke();

        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        OnItemChanged.Invoke();
    }
}
