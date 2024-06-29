using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObtainLeafArmor : MonoBehaviour
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
    bool isHeadEquipped = false;
    bool isChestEquipped = false;
    bool isLegsEquipped = false;

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
        if (newItem != null && newItem.name == "Leaf Headband")
        {
            HeadEquipped();
        }

        if (newItem != null && newItem.name == "Leaf Armband")
        {
            ChestEquipped();
        }

        if (newItem != null && newItem.name == "Leaf Skirt")
        {
            LegsEquipped();
        }
    }

    public void QuestAccepted()
    {
        if (npc.QuestIndex == questNumber)
        {
            state = questState.started;

            questTrackUI.SetTrackUI(quest);

            OnQuestAccepted?.Invoke();

            if (isHeadEquipped)
            {
                questTrackUI.Track1();
            }

            if (isChestEquipped)
            {
                questTrackUI.Track2();
            }

            if (isLegsEquipped)
            {
                questTrackUI.Track3();
            }

            if (isHeadEquipped && isChestEquipped && isLegsEquipped)
            {
                QuestCompleted();
            }
        }
    }

    void HeadEquipped()
    {
        if (state == questState.started)
        {
            isHeadEquipped = true;

            questTrackUI.Track1();

            QuestCompleted();
        }
        else
        {
            isHeadEquipped = true;
        }
    }

    void ChestEquipped()
    {
        if (state == questState.started)
        {
            isChestEquipped = true;

            questTrackUI.Track2();

            QuestCompleted();
        }
        else
        {
            isChestEquipped = true;
        }
    }

    void LegsEquipped()
    {
        if (state == questState.started)
        {
            isLegsEquipped = true;

            questTrackUI.Track3();

            QuestCompleted();
        }
        else
        {
            isLegsEquipped = true;
        }
    }

    public void QuestCompleted()
    {
        if (isHeadEquipped && isChestEquipped && isLegsEquipped)
        {
            state = questState.completed;

            isHeadEquipped = false;
            isChestEquipped = false;
            isLegsEquipped = false;

            OnQuestCompleted.Invoke();
        }
    }
}
