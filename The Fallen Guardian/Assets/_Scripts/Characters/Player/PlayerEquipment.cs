using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public bool IsWeaponEquipt = false;

    [SerializeField] GameObjectRuntimeSet playerInventoryReference;
    EquipmentManager equipmentManager;

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
                // If a weapon is equipped
                IsWeaponEquipt = true;

                UpdateWeaponSprite(newWeapon);
            }
        }
        else
        {
            // Unequipped the current item
            IsWeaponEquipt = false;
        }

        // Re-equipping a new item
        if (oldItem != null && oldItem is Weapon && newItem != null)
        {
            Weapon oldWeapon = oldItem as Weapon;
            Weapon newWeapon = newItem as Weapon;
            if (oldWeapon != null && newWeapon != null)
            {
                // If both the old and new items are weapons, consider it as re-equipping
                IsWeaponEquipt = true;

                UpdateWeaponSprite(newWeapon);
            }
        }
    }

    void UpdateWeaponSprite(Weapon equippedWeapon)
    {
        switch (equippedWeapon.weaponType)
        {
            case WeaponType.Sword:

                // Enabled the Sprite
                Sword.enabled = true;

                // Set Player Weapon Sprite
                Sword.sprite = equippedWeapon.weaponSprite;

                // Disable other weapon sprites and animators
                break;
            case WeaponType.Staff:

                // Enabled the Sprite
                Staff.enabled = true;

                // Set Player Weapon Sprite
                Staff.sprite = equippedWeapon.weaponSprite;

                // Disable other weapon sprites and animators
                break;
            case WeaponType.Bow:

                // Enabled the Sprite
                Bow.enabled = true;

                // Set Player Weapon Sprite
                Bow.sprite = equippedWeapon.weaponSprite;

                // Disable other weapon sprites and animators
                break;
            case WeaponType.Dagger:

                // Enabled the Sprite
                Dagger.enabled = true;

                // Set Player Weapon Sprite
                Dagger.sprite = equippedWeapon.weaponSprite;

                // Disable other weapon sprites and animators
                break;
        }
    }
}
