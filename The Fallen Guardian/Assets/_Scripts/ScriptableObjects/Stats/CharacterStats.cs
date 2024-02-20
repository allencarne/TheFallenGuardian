using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public int health;
    public int maxHealth;

    public int damage;

    public float movementSpeed;

    public float attackSpeed;
    public float coolDownReduction;
    public float protection;
    public float regeneration;
}
