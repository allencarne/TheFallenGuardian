using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestUI : MonoBehaviour
{
    [Header("Quest UI")]
    public GameObject QuestUI;

    [Header("Quest")]
    public TextMeshProUGUI QuestName;
    public TextMeshProUGUI QuestDialogue;

    [Header("Quest Objectives")]
    public TextMeshProUGUI QuestObjective1;
    public TextMeshProUGUI QuestObjective2;
    public TextMeshProUGUI QuestObjective3;
    public TextMeshProUGUI QuestObjective4;
    public TextMeshProUGUI QuestObjective5;

    [Header("Quest Reward 1")]
    public TextMeshProUGUI QuestReward1;
    public Image QuestReward1Icon;

    [Header("Quest Reward 2")]
    public TextMeshProUGUI QuestReward2;
    public Image QuestReward2Icon;

    [Header("Quest Reward 3")]
    public TextMeshProUGUI QuestReward3;
    public Image QuestReward3Icon;

    [Header("Quest Reward UI")]
    public GameObject QuestRewardUI;

    [Header("Return Quest")]
    public TextMeshProUGUI ReturnQuestName;
    public TextMeshProUGUI ReturnQuestDialogue;

    [Header("Return Quest Reward 1")]
    public TextMeshProUGUI ReturnReward1;
    public Image ReturnReward1Icon;

    [Header("Return Quest Reward 2")]
    public TextMeshProUGUI ReturnReward2;
    public Image ReturnReward2Icon;

    [Header("Return Quest Reward 3")]
    public TextMeshProUGUI ReturnReward3;
    public Image ReturnReward3Icon;

    public void SetupUI(Quest quest)
    {
        // Quest
        QuestName.text = quest.QuestName;
        QuestDialogue.text = quest.QuestDialogue;
        // Objectives
        QuestObjective1.text = quest.QuestObjective1;
        QuestObjective2.text = quest.QuestObjective2;
        QuestObjective3.text = quest.QuestObjective3;
        QuestObjective4.text = quest.QuestObjective4;
        QuestObjective5.text = quest.QuestObjective5;
        // Quest Reward 1
        QuestReward1.text = quest.QuestReward1;
        QuestReward1Icon.sprite = quest.QuestReward1Icon;
        // Quest Reward 2
        QuestReward2.text = quest.QuestReward2;
        QuestReward2Icon.sprite = quest.QuestReward2Icon;
        // Quest Reward 3
        QuestReward3.text = quest.QuestReward3;
        QuestReward3Icon.sprite = quest.QuestReward3Icon;

        // Return Quest
        ReturnQuestName.text = quest.QuestName;
        ReturnQuestDialogue.text = quest.QuestRewardDialogue;
        // Quest Reward 1
        ReturnReward1.text = quest.QuestReward1;
        ReturnReward1Icon.sprite = quest.QuestReward1Icon;
        // Quest Reward 2
        ReturnReward2.text = quest.QuestReward2;
        ReturnReward2Icon.sprite = quest.QuestReward2Icon;
        // Quest Reward 3
        ReturnReward3.text = quest.QuestReward3;
        ReturnReward3Icon.sprite = quest.QuestReward3Icon;

        if (quest.QuestReward2Icon == null)
        {
            QuestReward2Icon.enabled = false;
            ReturnReward2Icon.enabled = false;
        }
        else
        {
            QuestReward2Icon.enabled = enabled;
            ReturnReward2Icon.enabled = enabled;
        }

        if (quest.QuestReward3Icon == null)
        {
            QuestReward3Icon.enabled = false;
            ReturnReward3Icon.enabled = false;
        }
        else
        {
            QuestReward3Icon.enabled = enabled;
            ReturnReward3Icon.enabled = enabled;
        }
    }
}
