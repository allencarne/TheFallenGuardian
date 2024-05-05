using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillSomeSnails : MonoBehaviour
{
    [SerializeField] int questNumber;

    [Header("Components")]
    [SerializeField] QuestTrackUI questTrackUI;
    [SerializeField] Quest quest;
    [SerializeField] NPCQuestGiver npc;

    [Header("Variables")]
    bool snailsKilled = false;
    int count = 0;
    int amount = 10;

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

    public void QuestAccepted()
    {
        if (npc.QuestIndex == questNumber)
        {
            state = questState.started;

            questTrackUI.SetTrackCounterUI(quest, count, amount);

            OnQuestAccepted?.Invoke();
        }
    }

    public void SnailHit()
    {
        if (state == questState.started)
        {
            count++;

            questTrackUI.SetTrackCounterUI(quest, count, amount);

            if (count == amount)
            {
                snailsKilled = true;

                questTrackUI.Track1();

                QuestCompleted();
            }
        }
    }

    public void QuestCompleted()
    {
        if (snailsKilled)
        {
            state = questState.completed;

            snailsKilled = false;

            OnQuestCompleted.Invoke();
        }
    }
}
