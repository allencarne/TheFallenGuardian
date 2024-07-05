using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player")]
    public string PlayerName;
    public int PlayerLevel;
    public float CurrentExperience;
    public float RequiredExperience;
    public PlayerClass PlayerClass;
    public Sprite PlayerClassIcon;

    [Header("Health")]
    public float Health = 10;
    public float MaxHealth = 10;

    [Header("Movement Speed")]
    public float BaseSpeed = 8;
    public float CurrentSpeed = 8;

    [Header("Attack Damage")]
    public int BaseDamage = 1;
    public int CurrentDamage = 1;

    [Header("Attack Speed")]
    public float BaseAttackSpeed = 1;
    public float CurrentAttackSpeed = 1;

    [Header("Cool Down Reduction")]
    public float BaseCDR = 1;
    public float CurrentCDR = 1;

    [Header("Armor")]
    public float BaseArmor = 0;
    public float CurrentArmor = 0;

    [Header("Attributes")]
    public float Vitality; // Increase Health
    // Movement Speed is not an attribute
    public float Power; // Increase Damage
    public float Toughness; // Increase Armor (reduce damage taken)
    public float Quickness; // Increase Attack Speed
    public float Recharge; // Increase Cool Down Reduction

    [Header("Currency")]
    public int Gold;

    [Header("Fury")]
    public float Fury = 0;
    public float MaxFury = 100;
}

public enum PlayerClass 
{ 
    Beginner,
    Warrior,
    Magician,
    Archer,
    Rogue
}
