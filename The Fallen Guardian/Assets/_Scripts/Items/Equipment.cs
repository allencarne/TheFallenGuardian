using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "ScriptableObjects/Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentType equipmentType;

    public int healthModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();

        //EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}

public enum EquipmentType
{
    Head,
    Chest,
    Legs,
    Ring,
    Necklace,
    Weapon,
    Shoulder,
    Back
}
