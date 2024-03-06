using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public bool IsWeaponEquipt = false;

    [SerializeField] GameObjectRuntimeSet playerInventoryReference;
    EquipmentManager equipmentManager;

    [SerializeField] SpriteRenderer Sword;
    //[SerializeField] SpriteRenderer Staff;
    //[SerializeField] SpriteRenderer Bow;
    //[SerializeField] SpriteRenderer Dagger;

    [SerializeField] Animator SwordAnimator;
    //[SerializeField] Animator StaffAnimator;
    //[SerializeField] Animator BowAnimator;
    //[SerializeField] Animator DaggerAnimator;

    private void Start()
    {
        equipmentManager = playerInventoryReference.GetItemIndex(0).GetComponent<EquipmentManager>();
        equipmentManager.onEquipmentChangedCallback += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem == null)
            return;

        Weapon equippedWeapon = newItem as Weapon;

        if (equippedWeapon != null)
        {
            switch (equippedWeapon.weaponType)
            {
                case WeaponType.Sword:
                    Debug.Log("Sword is equipt");
                    // Sword is Equipped
                    IsWeaponEquipt = true;
                    Sword.enabled = true;
                    SwordAnimator.enabled = true;
                    Sword.sprite = equippedWeapon.weaponSprite;
                    // Disable other weapon sprites and animators
                    break;
                case WeaponType.Staff:
                    Debug.Log("Staff is equipt");
                    // Staff is Equipped
                    //Staff.enabled = true;
                    //StaffAnimator.enabled = true;
                    // Disable other weapon sprites and animators
                    break;
                case WeaponType.Bow:
                    Debug.Log("Bow is equipt");
                    // Bow is Equipped
                    //Bow.enabled = true;
                    //BowAnimator.enabled = true;
                    // Disable other weapon sprites and animators
                    break;
                case WeaponType.Dagger:
                    Debug.Log("Dagger is equipt");
                    // Dagger is Equipped
                    //Dagger.enabled = true;
                    //DaggerAnimator.enabled = true;
                    // Disable other weapon sprites and animators
                    break;
            }
        }
    }
}
