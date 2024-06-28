using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillSomeHermits : MonoBehaviour
{
    [SerializeField] int questNumber;

    [Header("Components")]
    [SerializeField] QuestTrackUI questTrackUI;
    [SerializeField] Quest quest;
    [SerializeField] NPCQuestGiver npc;

    [Header("Variables")]
    bool hermitsKilled = false;
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

    public void HermitKilled()
    {
        if (state == questState.started)
        {
            count++;

            questTrackUI.SetTrackCounterUI(quest, count, amount);

            if (count == amount)
            {
                hermitsKilled = true;

                questTrackUI.Track1();

                QuestCompleted();
            }
        }
    }

    public void QuestCompleted()
    {
        if (hermitsKilled)
        {
            state = questState.completed;

            hermitsKilled = false;

            OnQuestCompleted.Invoke();
        }
    }
}
