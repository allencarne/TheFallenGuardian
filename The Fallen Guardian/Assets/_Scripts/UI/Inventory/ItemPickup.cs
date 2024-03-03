using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerInventoryReference;

    [SerializeField] Item item;

    public void PickUp()
    {
        Debug.Log("Picking up item " + item.name);

        // Store Reference to Inventory
        Inventory playerInv = playerInventoryReference.GetItemIndex(0).GetComponent<Inventory>();

        // Add Item to Inventory if we have enough space
        bool wasPickedUp = playerInv.Add(item);

        // Destroy item if it was collected
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }

        // Event for sounds and other things
        //OnCoinCollected?.Invoke();
    }
}
