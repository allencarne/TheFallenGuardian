using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipmentManager;

public class PlayerEquipment : MonoBehaviour
{
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

    }
}
