using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LearnASecondAbility : MonoBehaviour
{
    [SerializeField] int questNumber;

    [Header("Components")]
    [SerializeField] QuestTrackUI questTrackUI;
    [SerializeField] Quest quest;
    [SerializeField] NPCQuestGiver npc;

    [Header("References")]
    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerAbilities abilities;

    [Header("Variables")]
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
        if (playerReference.items.Count > 0)
        {
            abilities = playerReference.GetItemIndex(0).GetComponent<PlayerAbilities>();
            abilities.OnOffensiveAbilityChanged += OnAbilityChanged;
        }
    }

    void OnAbilityChanged()
    {
        abilitySelected = true;

        AbilitySelected();
    }

    public void QuestAccepted()
    {
        if (npc.QuestIndex == questNumber)
        {
            state = questState.started;

            questTrackUI.SetTrackUI(quest);

            OnQuestAccepted?.Invoke();
        }
    }

    public void AbilityUIOpened()
    {
        if (state == questState.started)
        {
            abilityUIOpened = true;

            questTrackUI.Track1();

            QuestCompleted();
        }
    }

    public void AbilitySelected()
    {
        if (state == questState.started)
        {
            questTrackUI.Track2();

            QuestCompleted();
        }
    }

    public void QuestCompleted()
    {
        if (abilityUIOpened && abilitySelected)
        {
            state = questState.completed;

            abilityUIOpened = false;
            abilitySelected = false;

            OnQuestCompleted.Invoke();
        }
    }
}
