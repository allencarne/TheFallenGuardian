using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WhereAreYourClothes : MonoBehaviour
{
    [SerializeField] NPCQuestGiver npc;

    [Header("References")]
    [SerializeField] GameObjectRuntimeSet inventoryReference;
    EquipmentManager equipmentManager;
    Inventory inventory;

    [SerializeField] GameObject shirt;
    [SerializeField] GameObject shorts;

    public UnityEvent OnQuestCompleted;

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
        NotStarted,
        started,
        completed
    }

    questState state = questState.NotStarted;

    bool pickupShirt = false;
    bool pickupShorts = false;
    bool inventoryOpened = false;
    bool equipShirt = false;
    bool equipShorts = false;

    [SerializeField] TextMeshProUGUI questTrackName;
    [SerializeField] TextMeshProUGUI[] questTrackObjective;

    public void PickUpShirt()
    {
        if (state == questState.started)
        {
            pickupShirt = true;
        }
    }

    public void PickUpShorts()
    {
        if (state == questState.started)
        {
            pickupShorts = true;
        }
    }

    public void OnInventoryUIOpened()
    {
        if (state == questState.started)
        {
            inventoryOpened = true;
        }

        if (pickupShirt && pickupShorts && equipShirt && equipShorts && inventoryOpened)
        {
            state = questState.completed;

            OnQuestCompleted.Invoke();
        }
    }

    public void EquipShirt()
    {
        if (state == questState.started)
        {
            equipShirt = true;

            if (pickupShirt && pickupShorts && equipShirt && equipShorts && inventoryOpened)
            {
                state = questState.completed;

                OnQuestCompleted.Invoke();
            }
        }
    }

    public void EquipShorts()
    {
        if (state == questState.started)
        {
            equipShorts = true;

            if (pickupShirt && pickupShorts && equipShirt && equipShorts && inventoryOpened)
            {
                state = questState.completed;

                OnQuestCompleted.Invoke();
            }
        }
    }

    public void QuestAccepted()
    {
        state = questState.started;

        Instantiate(shirt, npc.rewardPosition.position, Quaternion.identity);

        Instantiate(shorts, npc.rewardPosition.position, Quaternion.identity);
    }
}
