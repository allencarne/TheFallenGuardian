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

    [Header("Variables")]
    bool stickEquipped = false;
    bool abilityUIOpened = false;
    bool abilitySelected = false;

    [Header("Events")]
    public UnityEvent OnQuestCompleted;

    enum questState
    {
        NotStarted,
        started,
        completed,
    }

    questState state = questState.NotStarted;

    public void QuestAccepted()
    {
        if (npc.QuestIndex == questNumber)
        {
            state = questState.started;

            questTrackUI.SetTrackUI(quest);
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
