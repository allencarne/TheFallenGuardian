using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NPCQuestGiver : MonoBehaviour
{
    [Header("NPC")]
    [SerializeField] Animator exclamationAnimator;
    [SerializeField] TextMeshProUGUI interactText;

    [Header("Quest")]
    public bool isQuestAccepted = false;
    public bool isQuestCompleted = false;
    [SerializeField] UnityEvent OnQuestAccepted;
    public Quest[] quests;
    int questIndex = 0;

    [Header("Quest UI")]
    [SerializeField] GameObject QuestUI;
    [SerializeField] GameObject QuestRewardUI;
    [SerializeField] TextMeshProUGUI npcName;
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDialogue;
    [SerializeField] TextMeshProUGUI questObjective;
    [SerializeField] TextMeshProUGUI questReward;

    [SerializeField] TextMeshProUGUI questRewardDialogue;

    [Header("Reward")]
    public Transform rewardPosition;
    LevelSystem levelSystem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.text = "Press " + "F" + " To Interact";

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

                SetupQuestUI(questIndex);

                QuestUI.SetActive(true);
            }
        }

        if (collision.CompareTag("Player"))
        {
            if (isQuestAccepted && isQuestCompleted)
            {
                if (collision.GetComponent<PlayerInputHandler>().InteractInput)
                {
                    interactText.text = "";

                    QuestRewardUI.SetActive(true);

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
            QuestUI.SetActive(false);
            QuestRewardUI.SetActive(false);

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }

    public void AcceptQuest()
    {
        OnQuestAccepted?.Invoke();

        isQuestAccepted = true;

        QuestUI.SetActive(false);

        exclamationAnimator.Play("NPC Question");
    }

    public void DeclineQuest()
    {
        isQuestAccepted = false;

        QuestUI.SetActive(false);
    }

    void SetupQuestUI(int index)
    {
        npcName.text = quests[index].NPCName;
        questName.text = quests[index].QuestName;
        questDialogue.text = quests[index].QuestDialogue;
        questObjective.text = quests[index].QuestObjective;
        questReward.text = quests[index].QuestReward;
        questRewardDialogue.text = quests[index].QuestRewardDialogue;
    }

    public void CompleteQuest()
    {
        Debug.Log("Completed?");
        isQuestCompleted = true;
    }

    public void CompleteButton(int index)
    {
        QuestRewardUI.SetActive(false);

        if (levelSystem != null)
        {
            levelSystem.GainExperienceFlatRate(quests[index].EXPReward);
        }
        Instantiate(quests[index].QuestRewardPrefab, rewardPosition);

        //
        isQuestAccepted = false;
        isQuestCompleted = false;
        questIndex++;
        exclamationAnimator.Play("NPC Exclamation");
    }

    public void CancelButton()
    {
        QuestRewardUI.SetActive(false);
    }
}
