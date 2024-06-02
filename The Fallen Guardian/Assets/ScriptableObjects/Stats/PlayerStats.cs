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
    public float Health;
    public float MaxHealth;

    [Header("Movement Speed")]
    public float BaseSpeed;
    public float CurrentSpeed;

    [Header("Attack Damage")]
    public int BaseDamage;
    public int CurrentDamage;
    public int activeMightAmount; // New variable for might amount
    public int activeWeaknessAmount; // New variable for weakness amount

    [Header("Attack Speed")]
    public float BaseAttackSpeed;
    public float CurrentAttackSpeed;

    [Header("Cool Down Reduction")]
    public float BaseCDR;
    public float CurrentCDR;

    [Header("Armor")]
    public float BaseArmor;
    public float CurrentArmor;

    [Header("Regen")]
    public float BaseRegen;
    public float CurrentRegen;

    [Header("Attributes")]
    public float Vitality; // Increase Health
    // Movement Speed is not an attribute
    public float Power; // Increase Damage
    public float Toughness; // Increase Armor (reduce damage taken)
    public float Quickness; // Increase Attack Speed
    public float Recharge; // Increase Cool Down Reduction

    [Header("Currency")]
    public int Gold;
}

public enum PlayerClass 
{ 
    Beginner,
    Warrior,
    Magician,
    Archer,
    Rogue
}
