using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public float health;
    public float maxHealth;
    public float movementSpeed;
    public float damage;
    public PlayerClass playerClass; // Possibly move this out of the characterstats and into the player
}

public enum PlayerClass
{
    Beginner,
    Warrior,
    Mage
}
