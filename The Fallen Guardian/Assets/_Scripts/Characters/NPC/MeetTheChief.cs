using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeetTheChief : MonoBehaviour
{
    [SerializeField] int questNumber;

    [Header("Components")]
    [SerializeField] NPCQuestUI npcQuestUI;
    [SerializeField] QuestTrackUI questTrackUI;
    [SerializeField] Quest quest;
    [SerializeField] NPCQuestGiver npc;

    [Header("Variables")]

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

    public void Accepted()
    {
        npc.isQuestAvaliable = true;
        npc.IsQuestAccepted = true;

        npcQuestUI.SetupUI(quest);
    }

    public void MeetCheif()
    {
        if (state == questState.started)
        {
            questTrackUI.Track1();

            QuestCompleted();
        }
    }

    public void QuestCompleted()
    {
        state = questState.completed;

        OnQuestCompleted.Invoke();
    }

    public void Test()
    {
        Debug.Log("Test");
    }
}
