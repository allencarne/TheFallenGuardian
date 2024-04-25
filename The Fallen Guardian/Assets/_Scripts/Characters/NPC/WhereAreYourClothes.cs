using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WhereAreYourClothes : MonoBehaviour
{
    [SerializeField] Quest quest;
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

    enum questState
    {
        NotStarted,
        started,
        completed,
        TurnedIn
    }

    questState state = questState.NotStarted;

    bool pickupShirt = false;
    bool pickupShorts = false;
    bool inventoryOpened = false;
    bool equipShirt = false;
    bool equipShorts = false;

    [SerializeField] TextMeshProUGUI questNameText;
    [SerializeField] TextMeshProUGUI questTrack1Text;
    [SerializeField] TextMeshProUGUI questTrack2Text;
    [SerializeField] TextMeshProUGUI questTrack3Text;
    [SerializeField] TextMeshProUGUI questTrack4Text;
    [SerializeField] TextMeshProUGUI questTrack5Text;

    public void PickUpShirt()
    {
        if (state == questState.started)
        {
            pickupShirt = true;

            questTrack1Text.fontStyle |= FontStyles.Strikethrough;
        }
    }

    public void PickUpShorts()
    {
        if (state == questState.started)
        {
            pickupShorts = true;

            questTrack2Text.fontStyle |= FontStyles.Strikethrough;
        }
    }

    public void OnInventoryUIOpened()
    {
        if (state == questState.started)
        {
            inventoryOpened = true;

            questTrack3Text.fontStyle |= FontStyles.Strikethrough;
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

            questTrack4Text.fontStyle |= FontStyles.Strikethrough;

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

            questTrack5Text.fontStyle |= FontStyles.Strikethrough;

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

        questNameText.text = quest.QuestName;
        questTrack1Text.text = quest.QuestObjective1;
        questTrack2Text.text = quest.QuestObjective2;
        questTrack3Text.text = quest.QuestObjective3;
        questTrack4Text.text = quest.QuestObjective4;
        questTrack5Text.text = quest.QuestObjective5;
    }

    public void QuestTurnedIn()
    {
        questNameText.text = "";
        questTrack1Text.text = "";
        questTrack2Text.text = "";
        questTrack3Text.text = "";
        questTrack4Text.text = "";
        questTrack5Text.text = "";
    }
}
