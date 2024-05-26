using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObjectRuntimeSet playerInventoryReference;
    EquipmentManager equipmentManager;

    [Header("Armor")]
    [SerializeField] SpriteRenderer headSprite;
    public int HeadIndex;
    [SerializeField] SpriteRenderer chestSprite;
    public int ChestIndex;
    [SerializeField] SpriteRenderer legsSprite;
    public int LegsIndex;

    [Header("Weapons")]
    [HideInInspector] public bool IsWeaponEquipt = false;
    [SerializeField] SpriteRenderer Sword;
    [SerializeField] SpriteRenderer Staff;
    [SerializeField] SpriteRenderer Bow;
    [SerializeField] SpriteRenderer Dagger;

    private void Start()
    {
        equipmentManager = playerInventoryReference.GetItemIndex(0).GetComponent<EquipmentManager>();
        equipmentManager.onEquipmentChangedCallback += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        // Equipped a new item
        if (newItem != null)
        {
            Weapon newWeapon = newItem as Weapon;
            if (newWeapon != null)
            {
                IsWeaponEquipt = true;
                EquipWeapon(newWeapon);
            }

            // Handle armor equip
            EquipArmor(newItem);
        }
        else
        {
            Weapon oldWeapon = oldItem as Weapon;
            if (oldWeapon != null)
            {
                IsWeaponEquipt = false;
                UnequipWeapon(oldWeapon); // Call method to disable all weapon sprites
            }

            // Handle armor unequip
            UnequipArmor(oldItem);
        }
    }

    void EquipWeapon(Weapon newWeapon)
    {
        //ResetWeaponSprites(); // Reset all weapon sprites before enabling the new one

        switch (newWeapon.weaponType)
        {
            case WeaponType.Sword:
                Sword.enabled = true;
                Sword.sprite = newWeapon.weaponSprite;
                break;
            case WeaponType.Staff:
                Staff.enabled = true;
                Staff.sprite = newWeapon.weaponSprite;
                break;
            case WeaponType.Bow:
                Bow.enabled = true;
                Bow.sprite = newWeapon.weaponSprite;
                break;
            case WeaponType.Dagger:
                Dagger.enabled = true;
                Dagger.sprite = newWeapon.weaponSprite;
                break;
        }
    }

    void UnequipWeapon(Weapon oldWeapon)
    {
        switch (oldWeapon.weaponType)
        {
            case WeaponType.Sword:
                Sword.enabled = false;
                //Sword.sprite = equippedWeapon.weaponSprite;
                break;
            case WeaponType.Staff:
                Staff.enabled = false;
                //Staff.sprite = equippedWeapon.weaponSprite;
                break;
            case WeaponType.Bow:
                Bow.enabled = false;
                //Bow.sprite = equippedWeapon.weaponSprite;
                break;
            case WeaponType.Dagger:
                Dagger.enabled = false;
                //Dagger.sprite = equippedWeapon.weaponSprite;
                break;
        }
    }

    void EquipArmor(Equipment newItem)
    {
        switch (newItem.equipmentType)
        {
            case EquipmentType.Head:
                switch (newItem.itemIndex)
                {
                    case 1:
                        // Tattered Headband Equipped
                        HeadIndex = newItem.itemIndex;
                        //isHeadEquipped = true;
                        headSprite.enabled = true;
                        break;
                    case 2:
                        // Leaf Headband Eqipped
                        HeadIndex = newItem.itemIndex;
                        //isHeadEquipped = true;
                        headSprite.enabled = true;
                        break;
                }
                break;
            case EquipmentType.Chest:
                switch (newItem.itemIndex)
                {
                    case 1:
                        // Tattered Shirt Equipped
                        ChestIndex = newItem.itemIndex;
                        chestSprite.enabled = true;
                        break;
                    case 2:
                        // Leaf Armband Eqipped
                        ChestIndex = newItem.itemIndex;
                        chestSprite.enabled = true;
                        break;
                }
                break;
            case EquipmentType.Legs:
                switch (newItem.itemIndex)
                {
                    case 1:
                        // Tattered Shorts Equipped
                        LegsIndex = newItem.itemIndex;
                        legsSprite.enabled = true;
                        break;
                    case 2:
                        // Leaf Skirt Eqipped
                        LegsIndex = newItem.itemIndex;
                        legsSprite.enabled = true;
                        break;
                }
                break;
                /*
            case EquipmentType.Ring:
                break;
            case EquipmentType.Necklace:
                break;
            case EquipmentType.Weapon:
                break;
            case EquipmentType.Shoulder:
                break;
            case EquipmentType.Back:
                break;
                */
        }
    }

    void UnequipArmor(Equipment oldItem)
    {
        if (oldItem != null)
        {
            switch (oldItem.equipmentType)
            {
                case EquipmentType.Head:
                    switch (oldItem.itemIndex)
                    {
                        case 1:
                            // Tattered Headband Unequipped
                            HeadIndex = 0; // Indicate that no headgear is equipped
                            headSprite.enabled = false;
                            break;
                        case 2:
                            // Leaf Headband Unequipped
                            HeadIndex = 0; // Indicate that no headgear is equipped
                            headSprite.enabled = false;
                            break;
                    }
                    break;
                case EquipmentType.Chest:
                    switch (oldItem.itemIndex)
                    {
                        case 1:
                            // Tattered Headband Unequipped
                            ChestIndex = 0; // Indicate that no headgear is equipped
                            chestSprite.enabled = false;
                            break;
                        case 2:
                            // Leaf Headband Unequipped
                            ChestIndex = 0; // Indicate that no headgear is equipped
                            chestSprite.enabled = false;
                            break;
                    }
                    break;
                case EquipmentType.Legs:
                    switch (oldItem.itemIndex)
                    {
                        case 1:
                            // Tattered Headband Unequipped
                            LegsIndex = 0; // Indicate that no headgear is equipped
                            legsSprite.enabled = false;
                            break;
                        case 2:
                            // Leaf Headband Unequipped
                            LegsIndex = 0; // Indicate that no headgear is equipped
                            legsSprite.enabled = false;
                            break;
                    }
                    break;
                    /*
                case EquipmentType.Ring:
                    break;
                case EquipmentType.Necklace:
                    break;
                case EquipmentType.Weapon:
                    break;
                case EquipmentType.Shoulder:
                    break;
                case EquipmentType.Back:
                    break;
                    */
            }
        }
    }
}
