using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] Item item;

    public void PickUp()
    {
        Destroy(gameObject);

        // Event for sounds and other things
        //OnCoinCollected?.Invoke();

        /*
         
        // If inventory space is not full, collect the coin
        bool wasPickedUp = Inventory.instance.Add(item);

        // Destroy coin if it was collected
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }

        */
    }
}
