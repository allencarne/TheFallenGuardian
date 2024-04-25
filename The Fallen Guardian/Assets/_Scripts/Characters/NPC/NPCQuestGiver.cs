using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NPCQuestGiver : MonoBehaviour
{
    public Quest[] Quests;

    [Header("Components")]
    [SerializeField] GameObject npcExclamation;
    [SerializeField] Animator exclamationAnimator;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] NPCQuestUI npcQuestUI;
    public Transform rewardPosition;
    LevelSystem levelSystem;

    [Header("Variables")]
    public bool isQuestAccepted = false;
    public bool isQuestCompleted = false;
    int questIndex = 0;

    [Header("Events")]
    [SerializeField] UnityEvent OnQuestAccepted;
    [SerializeField] UnityEvent OnQuestCompleted;

    private void Start()
    {
        CheckQuestAvailability();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Update Interact Text
            interactText.text = "Press <color=red>F</color> To Interact";
            // Get The Rigid Body of the Player
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            // Make sure the PlayerRB never sleeps
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If Quest is Not Accepted
        if (collision.CompareTag("Player") && !isQuestAccepted)
        {
            if (collision.GetComponent<PlayerInputHandler>().InteractInput)
            {
                // Reset Interact Text
                interactText.text = "";

                // Check if there are quests remaining
                if (questIndex < Quests.Length)
                {
                    // Sets the QuestUI Text Elements
                    npcQuestUI.SetupUI(Quests[questIndex]);
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
            if (isQuestAccepted && isQuestCompleted)
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
        if (collision.CompareTag("Player"))
        {
            // Reset Interact Text
            interactText.text = "";
            // Disable Quest UI
            npcQuestUI.QuestUI.SetActive(false);
            // Disabled Quest Reward UI
            npcQuestUI.QuestRewardUI.SetActive(false);
            // Get Player RB
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            // Reset Player RB Settings
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }

    public void AcceptQuest()
    {
        OnQuestAccepted?.Invoke();
        isQuestAccepted = true;
        npcQuestUI.QuestUI.SetActive(false);
        exclamationAnimator.Play("NPC InProgress");
    }

    public void DeclineQuest()
    {
        isQuestAccepted = false;
        npcQuestUI.QuestUI.SetActive(false);
    }

    public void CompleteQuest()
    {
        if (!isQuestCompleted)
        {
            isQuestCompleted = true;

            exclamationAnimator.Play("NPC Question");
        }
    }

    private void CheckQuestAvailability()
    {
        if (questIndex >= Quests.Length)
        {
            npcExclamation.SetActive(false);
        }
        else
        {
            npcExclamation.SetActive(true);
        }
    }

    public void CompleteButton(int index)
    {
        npcQuestUI.QuestRewardUI.SetActive(false);
        levelSystem?.GainExperienceFlatRate(Quests[index].EXPReward);
        Instantiate(Quests[index].QuestRewardPrefab, rewardPosition);
        isQuestAccepted = false;
        isQuestCompleted = false;
        questIndex++;
        exclamationAnimator.Play("NPC Exclamation");
        CheckQuestAvailability();
        OnQuestCompleted?.Invoke();
    }

    public void CancelButton()
    {
        npcQuestUI.QuestRewardUI.SetActive(false);
    }
}