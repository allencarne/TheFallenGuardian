using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LearnAnAbility : MonoBehaviour
{
    [SerializeField] int questNumber;

    [Header("Components")]
    [SerializeField] QuestTrackUI questTrackUI;
    [SerializeField] Quest quest;
    [SerializeField] NPCQuestGiver npc;

    [Header("References")]
    [SerializeField] GameObjectRuntimeSet inventoryReference;
    EquipmentManager equipmentManager;

    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerAbilities abilities;

    [Header("Variables")]
    bool stickEquipped = false;
    bool abilityUIOpened = false;
    bool abilitySelected = false;

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

            abilities = playerReference.GetItemIndex(0).GetComponent<PlayerAbilities>();
            abilities.OnBasicAbilityChanged += OnAbilityChanged;
        }
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        // Check if the new item is not null and its name matches the desired item ("Tattered Shirt" or "Tattered Shorts")
        if (newItem != null && newItem.name == "Green Wooden Stick")
        {
            StickEquipped();
        }
    }

    void OnAbilityChanged()
    {
        AbilitySelected();
    }

    public void QuestAccepted()
    {
        if (npc.QuestIndex == questNumber)
        {
            state = questState.started;

            questTrackUI.SetTrackUI(quest);

            OnQuestAccepted?.Invoke();

            if (stickEquipped)
            {
                questTrackUI.Track1();
            }

            if (abilitySelected)
            {
                questTrackUI.Track3();
            }
        }
    }

    public void StickEquipped()
    {
        if (state == questState.started)
        {
            stickEquipped = true;

            questTrackUI.Track1();

            QuestCompleted();
        }
        else
        {
            stickEquipped = true;
        }
    }

    public void AbilityUIOpened()
    {
        if (state == questState.started)
        {
            abilityUIOpened = true;

            questTrackUI.Track2();

            QuestCompleted();
        }
    }

    public void AbilitySelected()
    {
        if (state == questState.started)
        {
            abilitySelected = true;

            questTrackUI.Track3();

            QuestCompleted();
        }
        else
        {
            abilitySelected = true;
        }
    }

    public void QuestCompleted()
    {
        if (stickEquipped && abilityUIOpened && abilitySelected)
        {
            state = questState.completed;

            stickEquipped = false;
            abilityUIOpened = false;
            abilitySelected = false;

            OnQuestCompleted.Invoke();
        }
    }
}
