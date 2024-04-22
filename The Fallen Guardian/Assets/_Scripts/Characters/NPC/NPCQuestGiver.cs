using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCQuestGiver : MonoBehaviour
{
    [SerializeField] Animator exclamationAnimator;

    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] GameObject QuestUI;
    public bool isQuestAccepted = false;

    [SerializeField] TextMeshProUGUI questTrackName;
    [SerializeField] TextMeshProUGUI questTrackObjective;

    public Quest[] quests;
    int questIndex = 0;

    [SerializeField] TextMeshProUGUI npcName;
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDialogue;
    [SerializeField] TextMeshProUGUI questObjective;
    [SerializeField] TextMeshProUGUI questReward;

    [SerializeField] GameObject shirt;
    [SerializeField] GameObject shorts;
    [SerializeField] Transform rewardPosition;
    [SerializeField] Transform itemPosition;

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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.text = "";
            QuestUI.SetActive(false);

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }

    public void AcceptQuest()
    {
        isQuestAccepted = true;

        QuestUI.SetActive(false);

        questTrackName.enabled = true;
        questTrackObjective.enabled = true;

        exclamationAnimator.Play("NPC Question");

        if (questIndex == 0)
        {
            Instantiate(shirt, rewardPosition.position, Quaternion.identity);
            Instantiate(shorts, itemPosition.position, Quaternion.identity);
        }
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
    }
}
