using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NPCQuestGiver : MonoBehaviour
{
    public Quest[] Quests;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI exclamationText;

    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] NPCQuestUI npcQuestUI;
    public Transform RewardPosition;
    LevelSystem levelSystem;

    [Header("Variables")]
    public bool isQuestAvaliable = false;
    public bool IsQuestAccepted = false;
    public bool IsQuestCompleted = false;
    public int QuestIndex = 0;

    [Header("Events")]
    [SerializeField] UnityEvent OnQuestAccepted;
    [SerializeField] UnityEvent OnQuestCompleted;

    private void Start()
    {
        CheckQuestAvailability();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isQuestAvaliable)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            // Update Interact Text
            interactText.text = "Press <color=red>F</color> To Interact";
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isQuestAvaliable)
        {
            return;
        }

        // If Quest is Not Accepted
        if (collision.CompareTag("Player") && !IsQuestAccepted)
        {
            if (collision.GetComponent<PlayerInputHandler>().InteractInput)
            {
                // Reset Interact Text
                interactText.text = "";

                // Check if there are quests remaining
                if (QuestIndex < Quests.Length)
                {
                    // Sets the QuestUI Text Elements
                    npcQuestUI.SetupUI(Quests[QuestIndex]);
                    // Enables the Quest UI
                    npcQuestUI.QuestUI.SetActive(true);
                }
                else
                {
                    // No more quests, you can handle this situation here
                }
            }
        }

        // If Quest is Accepted && Completed
        if (collision.CompareTag("Player"))
        {
            if (IsQuestAccepted && IsQuestCompleted)
            {
                if (collision.GetComponent<PlayerInputHandler>().InteractInput)
                {
                    // Reset Interact Text
                    interactText.text = "";
                    // Enable Quest UI
                    npcQuestUI.QuestRewardUI.SetActive(true);
                    // Assign the Level System Reference
                    levelSystem = collision.GetComponent<LevelSystem>();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isQuestAvaliable)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            // Reset Interact Text
            interactText.text = "";
            // Disable Quest UI
            npcQuestUI.QuestUI.SetActive(false);
            // Disabled Quest Reward UI
            npcQuestUI.QuestRewardUI.SetActive(false);
        }
    }

    public void AcceptQuest()
    {
        OnQuestAccepted?.Invoke();
        IsQuestAccepted = true;
        npcQuestUI.QuestUI.SetActive(false);
        exclamationText.color = Color.gray;
        exclamationText.text = "?";
    }

    public void DeclineQuest()
    {
        IsQuestAccepted = false;
        npcQuestUI.QuestUI.SetActive(false);
    }

    public void CompleteQuest()
    {
        if (!IsQuestCompleted)
        {
            IsQuestCompleted = true;

            exclamationText.color = Color.yellow;
            exclamationText.text = "?";
        }
    }

    private void CheckQuestAvailability()
    {
        if (!isQuestAvaliable)
        {
            exclamationText.text = "";
            return;
        }

        if (QuestIndex >= Quests.Length)
        {
            exclamationText.text = "";
        }
        else
        {
            exclamationText.color = Color.yellow;
            exclamationText.text = "!";
        }
    }

    public void CompleteButton()
    {
        npcQuestUI.QuestRewardUI.SetActive(false);
        levelSystem?.GainExperienceFlatRate(Quests[QuestIndex].EXPReward);

        if (Quests[QuestIndex].QuestReward1Prefab != null)
        {
            Instantiate(Quests[QuestIndex].QuestReward1Prefab, RewardPosition);
        }

        IsQuestAccepted = false;
        IsQuestCompleted = false;
        QuestIndex++;
        CheckQuestAvailability();
        OnQuestCompleted?.Invoke();
    }

    public void CompleteHandoff()
    {
        IsQuestAccepted = false;
        IsQuestCompleted = false;
        QuestIndex++;
        CheckQuestAvailability();
    }

    public void CancelButton()
    {
        npcQuestUI.QuestRewardUI.SetActive(false);
    }
}