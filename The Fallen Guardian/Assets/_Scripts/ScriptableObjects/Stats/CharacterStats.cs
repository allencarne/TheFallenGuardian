using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public int health;
    public int maxHealth;
    public float movementSpeed;
    public int damage;
}
