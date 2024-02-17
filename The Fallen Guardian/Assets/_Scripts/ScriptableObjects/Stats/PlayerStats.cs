using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float health;
    public float maxHealth;
    public float movementSpeed;
    public float damage;
    public PlayerClass playerClass;

    // Current EXP
    // Max EXP
}

public enum PlayerClass 
{
    Beginner,
    Warrior,
    Mage
}

