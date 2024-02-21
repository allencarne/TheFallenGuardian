using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int health;
    public int maxHealth;
    public float movementSpeed;
    public int damage;
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
