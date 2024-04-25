using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WhereAreYourClothes : MonoBehaviour
{
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

            questTrackUI.QuestTrack1Text.fontStyle |= FontStyles.Strikethrough;
            questTrackUI.QuestTrack1Text.color = Color.black;
        }
    }

    public void PickUpShorts()
    {
        if (state == questState.started)
        {
            pickupShorts = true;

            questTrackUI.QuestTrack2Text.fontStyle |= FontStyles.Strikethrough;
            questTrackUI.QuestTrack2Text.color = Color.black;
        }
    }

    public void OnInventoryUIOpened()
    {
        if (state == questState.started)
        {
            inventoryOpened = true;

            questTrackUI.QuestTrack3Text.fontStyle |= FontStyles.Strikethrough;
            questTrackUI.QuestTrack3Text.color = Color.black;
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

            questTrackUI.QuestTrack4Text.fontStyle |= FontStyles.Strikethrough;
            questTrackUI.QuestTrack4Text.color = Color.black;

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

            questTrackUI.QuestTrack5Text.fontStyle |= FontStyles.Strikethrough;
            questTrackUI.QuestTrack5Text.color = Color.black;

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

        questTrackUI.SetTrackUI(quest);
    }
}
