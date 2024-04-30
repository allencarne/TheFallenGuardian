using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitADummy : MonoBehaviour
{
    [SerializeField] int questNumber;

    [Header("Components")]
    [SerializeField] QuestTrackUI questTrackUI;
    [SerializeField] Quest quest;
    [SerializeField] NPCQuestGiver npc;

    [Header("Variables")]
    bool dummyHit = false;

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

            questTrackUI.SetTrackUI(quest);

            OnQuestAccepted?.Invoke();
        }
    }

    public void DummyHit()
    {
        if (state == questState.started)
        {
            dummyHit = true;

            questTrackUI.Track1();

            QuestCompleted();
        }
    }

    public void QuestCompleted()
    {
        if (dummyHit)
        {
            state = questState.completed;

            dummyHit = false;

            OnQuestCompleted.Invoke();
        }
    }
}
