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
                UpdateWeaponSprite(newWeapon);
            }

            if (newItem.equipmentType == EquipmentType.Head)
            {
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
            }

            if (newItem.equipmentType == EquipmentType.Chest)
            {
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
            }

            if (newItem.equipmentType == EquipmentType.Legs)
            {
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
            }
        }
        else
        {
            // Unequipped the current item
            IsWeaponEquipt = false;
            ResetWeaponSprites(); // Call method to disable all weapon sprites

            headSprite.enabled = false;
            chestSprite.enabled = false;
            legsSprite.enabled = false;
        }
    }

    void UpdateWeaponSprite(Weapon equippedWeapon)
    {
        ResetWeaponSprites(); // Reset all weapon sprites before enabling the new one

        switch (equippedWeapon.weaponType)
        {
            case WeaponType.Sword:
                Sword.enabled = true;
                Sword.sprite = equippedWeapon.weaponSprite;
                break;
            case WeaponType.Staff:
                Staff.enabled = true;
                Staff.sprite = equippedWeapon.weaponSprite;
                break;
            case WeaponType.Bow:
                Bow.enabled = true;
                Bow.sprite = equippedWeapon.weaponSprite;
                break;
            case WeaponType.Dagger:
                Dagger.enabled = true;
                Dagger.sprite = equippedWeapon.weaponSprite;
                break;
        }
    }

    void ResetWeaponSprites()
    {
        Sword.enabled = false;
        //Staff.enabled = false;
        //Bow.enabled = false;
        //Dagger.enabled = false;
    }
}
