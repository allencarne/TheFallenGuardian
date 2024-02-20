using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public string playerName;
    public int playerLevel;

    public float playerEXP;
    public float playerMaxEXP;

    public PlayerClass playerClass;
}

public enum PlayerClass 
{ 
    Beginner,
    Warrior,
    Magician,
    Archer,
    Rogue
}
