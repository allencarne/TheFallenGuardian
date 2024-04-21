using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObjects/Quests")]
public class Quest : ScriptableObject
{
    public string NPCName;

    public string QuestName;
    [Multiline] // Allows for multi-line input in the Unity inspector
    public string QuestDialogue;
    [Multiline]
    public string QuestObjective;
    [Multiline]
    public string QuestReward;
    public GameObject QuestRewardPrefab;
}
