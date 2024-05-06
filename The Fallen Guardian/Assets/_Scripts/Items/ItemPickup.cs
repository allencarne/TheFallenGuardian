using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;
    [SerializeField] GameObjectRuntimeSet InventoryReference;
    Inventory inventory;

    [SerializeField] Item item;

    [SerializeField] TextMeshProUGUI pickupText;

    public UnityEvent OnCoinCollected;

    private void Start()
    {
        // Store Reference to Inventory
        inventory = InventoryReference.GetItemIndex(0).GetComponent<Inventory>();
    }

    public void PickUp()
    {
        if (item.isCurrency)
        {
            playerStats.Gold += 1;
            Destroy(gameObject);

            OnCoinCollected?.Invoke();
            return;
        }

        // Add Item to Inventory if we have enough space
        bool wasPickedUp = inventory.Add(item);

        // Destroy item if it was collected
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickupText.text = "Press <color=red>Z</color> To Pickup";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            pickupText.text = "";
        }
    }
}
