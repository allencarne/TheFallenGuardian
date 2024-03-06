using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Inventory/Equipment/Weapon")]
public class Weapon : Equipment
{
    public Sprite weaponSprite;
    public WeaponType weaponType;
}

public enum WeaponType
{
    Sword,
    Staff,
    Bow,
    Dagger
}