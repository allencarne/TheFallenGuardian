using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhereAreYourClothes : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObjectRuntimeSet inventoryReference;
    EquipmentManager equipmentManager;
    Inventory inventory;

    public void OnPlayerJoin()
    {
        Invoke("GetPlayer", .5f);
    }

    void GetPlayer()
    {
        if (inventoryReference.items.Count > 0)
        {
            equipmentManager = inventoryReference.GetItemIndex(0).GetComponent<EquipmentManager>();
            equipmentManager.onEquipmentChangedCallback += OnEquipmentChanged;

            inventory = inventoryReference.GetItemIndex(0).GetComponent<Inventory>();
            inventory.onItemChangedCallback += OnItemChanged;
        }
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        // Check if the new item is not null and its name matches the desired item ("Tattered Shirt" or "Tattered Shorts")
        if (newItem != null && newItem.name == "Tattered Shirt")
        {
            // Handle the case when the "Tattered Shirt" is equipped
            Debug.Log("Tattered Shirt equipped");
            // Add your logic here, such as triggering an event or updating a quest state
            EquipShirt();
        }
        else if (newItem != null && newItem.name == "Tattered Shorts")
        {
            // Handle the case when the "Tattered Shorts" are equipped
            Debug.Log("Tattered Shorts equipped");
            // Add your logic here for the "Tattered Shorts" item
            EquipShorts();
        }
    }

    private void OnItemChanged()
    {
        // Check if the inventory is not null and contains items
        if (inventory != null && inventory.items != null)
        {
            // Loop through all items in the inventory
            foreach (Item item in inventory.items)
            {
                // Check if the item is not null and its name matches the desired item
                if (item != null && item.name == "Tattered Shirt")
                {
                    Debug.Log("Shirt");
                    PickUpShirt();
                    break; // Exit the loop since we found the desired item
                }
            }

            // Repeat the same process for the "Tattered Shorts" item
            foreach (Item item in inventory.items)
            {
                if (item != null && item.name == "Tattered Shorts")
                {
                    Debug.Log("Shorts");
                    PickUpShorts();
                    break;
                }
            }
        }
    }

    enum questState
    {
        started,
        completed
    }

    questState state = questState.started;

    public UnityEvent OnPickupShirt;
    bool pickupShirt = false;
    public UnityEvent OnPickupShorts;
    bool pickupShorts = false;

    bool inventoryOpened = false;

    public UnityEvent OnEquipShirt;
    bool equipShirt = false;
    public UnityEvent OnEquipShorts;
    bool equipShorts = false;

    public void PickUpShirt()
    {
        pickupShirt = true;
    }

    public void PickUpShorts()
    {
        pickupShorts = true;
    }

    public void OnInventoryUIOpened()
    {
        inventoryOpened = true;
    }

    public void EquipShirt()
    {
        equipShirt = true;

        if (pickupShirt && pickupShorts && equipShirt && equipShorts && inventoryOpened)
        {
            state = questState.completed;
        }
    }

    public void EquipShorts()
    {
        equipShorts = true;

        if (pickupShirt && pickupShorts && equipShirt && equipShorts && inventoryOpened)
        {
            state = questState.completed;
        }
    }
}
