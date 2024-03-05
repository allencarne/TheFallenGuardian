using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] EquipmentManager equipmentManager;

    public Transform equipmentItemsParent;
    EquipmentSlot[] equipmentSlots;

    private void Start()
    {
        equipmentManager.onEquipmentChangedCallback += UpdateUI;

        equipmentSlots = equipmentItemsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    void UpdateUI(Equipment newItem, Equipment oldItem)
    {
        int slotIndex = (int)newItem.equipmentType;

        if (newItem != null)
        {
            equipmentSlots[slotIndex].AddItem(newItem);
        }
        else
        {
            equipmentSlots[slotIndex].ClearSlot();
        }
    }
}
