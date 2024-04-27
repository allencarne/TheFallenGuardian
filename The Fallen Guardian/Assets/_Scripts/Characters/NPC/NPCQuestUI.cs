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
    public TextMeshProUGUI NpcName;
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

    public TextMeshProUGUI NpcRewardName;
    public TextMeshProUGUI QuestRewardName;
    public TextMeshProUGUI QuestRewardDialogue;

    public TextMeshProUGUI QuestRewardText;
    public Image QuestRewardUIIcon;

    public void SetupUI(Quest quest)
    {
        // Quest
        NpcName.text = quest.NPCName;
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

        if (quest.QuestReward2Icon == null)
        {
            QuestReward2Icon.enabled = false;
        }

        if (quest.QuestReward3Icon == null)
        {
            QuestReward3Icon.enabled = false;
        }

        NpcRewardName.text = quest.NPCName;
        QuestRewardName.text = quest.QuestName;
        QuestRewardDialogue.text = quest.QuestRewardDialogue;
        QuestRewardText.text = quest.QuestReward1;
        QuestRewardUIIcon.sprite = quest.QuestReward1Icon;
    }
}
