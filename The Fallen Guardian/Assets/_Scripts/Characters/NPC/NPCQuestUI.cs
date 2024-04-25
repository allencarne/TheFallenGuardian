using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCQuestUI : MonoBehaviour
{
    [Header("Quest UI")]
    public GameObject QuestUI;

    public TextMeshProUGUI NpcName;
    public TextMeshProUGUI QuestName;
    public TextMeshProUGUI QuestDialogue;

    public TextMeshProUGUI QuestObjective1;
    public TextMeshProUGUI QuestObjective2;
    public TextMeshProUGUI QuestObjective3;
    public TextMeshProUGUI QuestObjective4;
    public TextMeshProUGUI QuestObjective5;

    public TextMeshProUGUI QuestReward;
    public Image QuestRewardIcon;

    [Header("Quest Reward UI")]
    public GameObject QuestRewardUI;

    public TextMeshProUGUI NpcRewardName;
    public TextMeshProUGUI QuestRewardName;
    public TextMeshProUGUI QuestRewardDialogue;

    public TextMeshProUGUI QuestRewardText;
    public Image QuestRewardUIIcon;

    public void SetupUI(Quest quest)
    {
        NpcName.text = quest.NPCName;
        QuestName.text = quest.QuestName;
        QuestDialogue.text = quest.QuestDialogue;

        QuestObjective1.text = quest.QuestObjective1;
        QuestObjective2.text = quest.QuestObjective2;
        QuestObjective3.text = quest.QuestObjective3;
        QuestObjective4.text = quest.QuestObjective4;
        QuestObjective5.text = quest.QuestObjective5;

        QuestReward.text = quest.QuestReward;
        QuestRewardIcon.sprite = quest.QuestRewardIcon;

        NpcRewardName.text = quest.NPCName;
        QuestRewardName.text = quest.QuestName;
        QuestRewardDialogue.text = quest.QuestRewardDialogue;
        QuestRewardText.text = quest.QuestReward;
        QuestRewardUIIcon.sprite = quest.QuestRewardIcon;
    }
}
