using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObjects/Quests")]
public class Quest : ScriptableObject
{
    public string QuestName;
    [Multiline]
    public string QuestDialogue;

    [Header("Objectives")]
    public string QuestObjective1;
    public string QuestObjective2;
    public string QuestObjective3;
    public string QuestObjective4;
    public string QuestObjective5;

    [Header("Quest Reward 1")]
    public string QuestReward1;
    public GameObject QuestReward1Prefab;
    public Sprite QuestReward1Icon;

    [Header("Quest Reward 2")]
    public string QuestReward2;
    public GameObject QuestReward2Prefab;
    public Sprite QuestReward2Icon;

    [Header("Quest Reward 3")]
    public string QuestReward3;
    public GameObject QuestReward3Prefab;
    public Sprite QuestReward3Icon;

    [Header("Reward Dialogue")]
    [Multiline]
    public string QuestRewardDialogue;

    public int EXPReward;
    public int GoldReward;
}
