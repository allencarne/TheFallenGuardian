using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] Animator headAnimator;

    [HideInInspector] public bool IsWeaponEquipt = false;

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
        // Reset all helmet layers
        ResetHelmetLayers();

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
                    case 0:
                        // Tattered Headband Equipped
                        Debug.Log("Tattered Headband Equipped");
                        headAnimator.SetLayerWeight(newItem.itemIndex - 1, 0);
                        headAnimator.SetLayerWeight(newItem.itemIndex, 1);
                        break;
                    case 1:
                        // Leaf Headband Eqipped
                        Debug.Log("Leaf Headband Eqipped");
                        headAnimator.SetLayerWeight(newItem.itemIndex + 1, 0);
                        headAnimator.SetLayerWeight(newItem.itemIndex, 1);
                        break;
                }
            }
        }
        else
        {
            // Unequipped the current item
            IsWeaponEquipt = false;
            ResetWeaponSprites(); // Call method to disable all weapon sprites
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

    void ResetHelmetLayers()
    {
        // Reset all helmet layers to weight 0
        for (int i = 0; i < headAnimator.layerCount; i++)
        {
            headAnimator.SetLayerWeight(i, 0);
        }
    }
}
