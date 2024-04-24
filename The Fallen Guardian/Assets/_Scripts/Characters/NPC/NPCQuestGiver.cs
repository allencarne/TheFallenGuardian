using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

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
    [SerializeField] TextMeshProUGUI npcName;
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDialogue;

    [SerializeField] TextMeshProUGUI questObjective1;
    [SerializeField] TextMeshProUGUI questObjective2;
    [SerializeField] TextMeshProUGUI questObjective3;
    [SerializeField] TextMeshProUGUI questObjective4;
    [SerializeField] TextMeshProUGUI questObjective5;
    [SerializeField] TextMeshProUGUI questReward;

    [SerializeField] Image questRewardIcon;

    [Header("Quest Reward UI")]
    [SerializeField] GameObject QuestRewardUI;
    [SerializeField] TextMeshProUGUI npcRewardName;
    [SerializeField] TextMeshProUGUI questRewardName;
    [SerializeField] TextMeshProUGUI questRewardDialogue;
    [SerializeField] TextMeshProUGUI questRewardText;
    [SerializeField] Image questRewardUIIcon;

    [Header("Reward")]
    public Transform rewardPosition;
    LevelSystem levelSystem;

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

        questObjective1.text = quests[index].QuestObjective1;
        questObjective2.text = quests[index].QuestObjective2;
        questObjective3.text = quests[index].QuestObjective3;
        questObjective4.text = quests[index].QuestObjective4;
        questObjective5.text = quests[index].QuestObjective5;

        questReward.text = quests[index].QuestReward;
        questRewardIcon.sprite = quests[index].QuestRewardIcon;

        npcRewardName.text = quests[index].NPCName;
        questRewardName.text = quests[index].QuestName;
        questRewardDialogue.text = quests[index].QuestRewardDialogue;
        questRewardText.text = quests[index].QuestReward;
        questRewardUIIcon.sprite = quests[index].QuestRewardIcon;
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
