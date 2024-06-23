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

    [Header("Variables")]
    bool isClubPurchased = false;
    bool isClubbEquipped = false;
    bool isStickSold = false;

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

    public void ClubPurchased()
    {
        if (state == questState.started)
        {
            isClubPurchased = true;

            questTrackUI.Track1();

            QuestCompleted();
        }
    }

    public void QuestCompleted()
    {
        if (isClubPurchased)
        {
            state = questState.completed;

            isClubPurchased = false;

            OnQuestCompleted.Invoke();
        }
    }
}
