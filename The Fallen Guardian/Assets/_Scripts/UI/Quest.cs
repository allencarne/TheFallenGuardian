using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObjects/Quests")]
public class Quest : ScriptableObject
{
    public string NPCName;

    public string QuestName;
    [Multiline]
    public string QuestDialogue;

    public string QuestObjective1;
    public string QuestObjective2;
    public string QuestObjective3;
    public string QuestObjective4;
    public string QuestObjective5;

    [Multiline]
    public string QuestReward;
    [Multiline]
    public string QuestRewardDialogue;

    public GameObject QuestRewardPrefab;
    public Sprite QuestRewardIcon;
    public int EXPReward;
    public int GoldReward;
}
