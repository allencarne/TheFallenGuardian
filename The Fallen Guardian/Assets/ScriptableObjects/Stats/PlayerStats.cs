using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player")]
    public string PlayerName;
    public int PlayerLevel;
    public float CurrentExperience;
    public float RequiredExperience;
    public PlayerClass PlayerClass;

    [Header("Character")]
    public float Health;
    public float MaxHealth;

    [Header("Stats")]
    public int Might; // Increase Attack Damage
    public float Haste; // Increase Movment Speed
    public float Agility; // Increase Attack Speed
    public float Alacrity; // Increase Cool Down Reduction
    public float Protection; // Increase Max Health / Reduce Incoming Damage
}

public enum PlayerClass 
{ 
    Beginner,
    Warrior,
    Magician,
    Archer,
    Rogue
}
