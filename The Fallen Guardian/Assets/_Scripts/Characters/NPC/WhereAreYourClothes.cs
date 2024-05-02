using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhereAreYourClothes : MonoBehaviour
{
    [SerializeField] int questNumber;

    [Header("Components")]
    [SerializeField] QuestTrackUI questTrackUI;
    [SerializeField] Quest quest;
    [SerializeField] NPCQuestGiver npc;
    [SerializeField] GameObject shirt;
    [SerializeField] GameObject shorts;

    [Header("References")]
    [SerializeField] GameObjectRuntimeSet inventoryReference;
    EquipmentManager equipmentManager;
    Inventory inventory;

    [Header("Variables")]
    bool pickupShirt = false;
    bool pickupShorts = false;
    bool inventoryOpened = false;
    bool equipShirt = false;
    bool equipShorts = false;

    [Header("Events")]
    public UnityEvent OnQuestAccepted;
    public UnityEvent OnQuestCompleted;

    enum questState
    {
        NotStarted,
        started,
        completed,
    }

    questState state = questState.NotStarted;

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
            EquipShirt();
        }
        else if (newItem != null && newItem.name == "Tattered Shorts")
        {
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
                    PickUpShirt();
                    break;
                }
            }

            // Repeat the same process for the "Tattered Shorts" item
            foreach (Item item in inventory.items)
            {
                if (item != null && item.name == "Tattered Shorts")
                {
                    PickUpShorts();
                    break;
                }
            }
        }
    }

    public void PickUpShirt()
    {
        if (state == questState.started)
        {
            pickupShirt = true;

            questTrackUI.Track1();
        }
    }

    public void PickUpShorts()
    {
        if (state == questState.started)
        {
            pickupShorts = true;

            questTrackUI.Track2();
        }
    }

    public void OnInventoryUIOpened()
    {
        if (state == questState.started)
        {
            inventoryOpened = true;

            questTrackUI.Track3();

            QuestCompleted();
        }
    }

    public void EquipShirt()
    {
        if (state == questState.started)
        {
            equipShirt = true;

            questTrackUI.Track4();

            QuestCompleted();
        }
    }

    public void EquipShorts()
    {
        if (state == questState.started)
        {
            equipShorts = true;

            questTrackUI.Track5();

            QuestCompleted();
        }
    }

    public void QuestAccepted()
    {
        if (npc.QuestIndex == questNumber)
        {
            state = questState.started;

            Instantiate(shirt, npc.RewardPosition.position, Quaternion.identity);

            Instantiate(shorts, npc.RewardPosition.position, Quaternion.identity);

            questTrackUI.SetTrackUI(quest);

            OnQuestAccepted?.Invoke();
        }
    }

    public void QuestCompleted()
    {
        if (pickupShirt && pickupShorts && equipShirt && equipShorts && inventoryOpened)
        {
            state = questState.completed;

            pickupShirt = false;
            pickupShorts = false;
            inventoryOpened = false;
            equipShirt = false;
            equipShorts = false;

            OnQuestCompleted.Invoke();
        }
    }
}
