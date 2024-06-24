using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuyAClub : MonoBehaviour
{
    [SerializeField] int questNumber;

    [Header("Components")]
    [SerializeField] QuestTrackUI questTrackUI;
    [SerializeField] Quest quest;
    [SerializeField] NPCQuestGiver npc;

    [Header("References")]
    [SerializeField] GameObjectRuntimeSet inventoryReference;
    EquipmentManager equipmentManager;

    [Header("Variables")]
    bool isClubPurchased = false;
    bool isClubbEquipped = false;
    public bool isStickSold = false;

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
        }
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        // Check if the new item is not null and its name matches the desired item ("Tattered Shirt" or "Tattered Shorts")
        if (newItem != null && newItem.name == "Club")
        {
            ClubEquipped();
        }
    }

    public void QuestAccepted()
    {
        if (npc.QuestIndex == questNumber)
        {
            state = questState.started;

            questTrackUI.SetTrackUI(quest);

            OnQuestAccepted?.Invoke();

            if (isClubPurchased)
            {
                questTrackUI.Track1();
            }

            if (isClubbEquipped)
            {
                questTrackUI.Track2();
            }

            if (isStickSold)
            {
                questTrackUI.Track3();
            }

            if (isClubPurchased && isClubbEquipped && isStickSold)
            {
                QuestCompleted();
            }
        }
    }

    public void ClubPurchased()
    {
        if (state == questState.started)
        {
            isClubPurchased = true;

            questTrackUI.Track1();

            QuestCompleted();
        }
        else
        {
            isClubPurchased = true;
        }
    }

    public void ClubEquipped()
    {
        if (state == questState.started)
        {
            isClubbEquipped = true;

            questTrackUI.Track2();

            QuestCompleted();
        }
        else
        {
            isClubbEquipped = true;
        }
    }

    public void StickSold()
    {
        if (state == questState.started)
        {
            isStickSold = true;

            questTrackUI.Track3();

            QuestCompleted();
        }
        else
        {
            isStickSold = true;
        }
    }

    public void QuestCompleted()
    {
        if (isClubPurchased && isClubbEquipped && isStickSold)
        {
            state = questState.completed;

            isClubPurchased = false;
            isClubbEquipped = false;
            isStickSold = false;

            OnQuestCompleted.Invoke();
        }
    }
}
