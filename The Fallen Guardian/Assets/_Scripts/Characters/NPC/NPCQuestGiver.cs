using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NPCQuestGiver : MonoBehaviour
{
    [Header("NPC")]
    [SerializeField] GameObject npcExclamation;
    [SerializeField] Animator exclamationAnimator;
    [SerializeField] TextMeshProUGUI interactText;

    [Header("Quest")]
    public bool isQuestAccepted = false;
    public bool isQuestCompleted = false;
    [SerializeField] UnityEvent OnQuestAccepted;
    [SerializeField] UnityEvent OnQuestCompleted;
    public Quest[] quests;
    int questIndex = 0;

    [Header("Reward")]
    public Transform rewardPosition;
    LevelSystem levelSystem;

    [SerializeField] NPCQuestUI npcQuestUI;

    private void Start()
    {
        CheckQuestAvailability();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.text = "Press <color=red>F</color> To Interact";

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isQuestAccepted)
        {
            if (collision.GetComponent<PlayerInputHandler>().InteractInput)
            {
                interactText.text = "";

                // Check if there are quests remaining
                if (questIndex < quests.Length)
                {
                    npcQuestUI.SetupUI(quests[questIndex]);
                    npcQuestUI.QuestUI.SetActive(true);
                }
                else
                {
                    // No more quests, you can handle this situation here
                    Debug.Log("No more quests available.");
                    // For example, you can deactivate NPC interaction
                    // gameObject.SetActive(false);
                }
            }
        }

        if (collision.CompareTag("Player"))
        {
            if (isQuestAccepted && isQuestCompleted)
            {
                if (collision.GetComponent<PlayerInputHandler>().InteractInput)
                {
                    interactText.text = "";

                    npcQuestUI.QuestRewardUI.SetActive(true);

                    levelSystem = collision.GetComponent<LevelSystem>();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.text = "";
            npcQuestUI.QuestUI.SetActive(false);
            npcQuestUI.QuestRewardUI.SetActive(false);

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
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
        if (questIndex >= quests.Length)
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

        if (levelSystem != null)
        {
            levelSystem.GainExperienceFlatRate(quests[index].EXPReward);
        }
        Instantiate(quests[index].QuestRewardPrefab, rewardPosition);

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
