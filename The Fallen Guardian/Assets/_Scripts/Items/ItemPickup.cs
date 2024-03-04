using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerInventoryReference;
    Inventory inventory;
    Inventory inventory2;

    [SerializeField] Item item;

    private void Start()
    {
        // Store Reference to Inventory
        inventory = playerInventoryReference.GetItemIndex(0).GetComponent<Inventory>();
        inventory2 = playerInventoryReference.GetItemIndex(1).GetComponent<Inventory>();
    }

    public void PickUp()
    {
        Debug.Log("Picking up item " + item.name);

        // Add Item to Inventory if we have enough space
        bool wasPickedUp = inventory.Add(item);

        // Destroy item if it was collected
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }

        // Event for sounds and other things
        //OnCoinCollected?.Invoke();
    }

    public void PickUp2()
    {
        Debug.Log("@@@Picking up item " + item.name);

        // Add Item to Inventory if we have enough space
        bool wasPickedUp = inventory2.Add(item);

        // Destroy item if it was collected
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }

        // Event for sounds and other things
        //OnCoinCollected?.Invoke();
    }
}
