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
        if (oldItem != null && oldItem is Weapon)
        {
            IsWeaponEquipt = false;
        }
        else if (newItem != null)
        {
            Weapon equippedWeapon = newItem as Weapon;

            if (equippedWeapon != null)
            {
                IsWeaponEquipt = true;

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
    }
}
