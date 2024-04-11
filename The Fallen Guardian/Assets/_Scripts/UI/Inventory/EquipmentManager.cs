using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentManager : MonoBehaviour
{
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChangedCallback;

    [SerializeField] Inventory inventory;
    [SerializeField] PlayerStats playerStats;

    // Array of all the items we currently have equipt
    public Equipment[] currentEquipment;

    private void Start()
    {
        // Get the number of equipment slots
        int numberOfSlots = System.Enum.GetNames(typeof(EquipmentType)).Length;

        // Define Array Length
        currentEquipment = new Equipment[numberOfSlots];
    }

    public void Equip(Equipment newItem)
    {
        // Gets the index that the new item is supposed to be slotted into
        int slotIndex = (int)newItem.equipmentType;

        Equipment oldItem = null;

        // If there is already a piece of equipment in the slot
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        playerStats.MaxHealth += newItem.healthModifier;
        playerStats.BaseDamage += newItem.damageModifier;

        if (onEquipmentChangedCallback != null)
        {
            onEquipmentChangedCallback.Invoke(newItem, oldItem);
        }

        // Set new item
        currentEquipment[slotIndex] = newItem;
    }

    public void UnEquip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            playerStats.MaxHealth -= oldItem.healthModifier;
            playerStats.BaseDamage -= oldItem.damageModifier;

            currentEquipment[slotIndex] = null;

            if (onEquipmentChangedCallback != null)
            {
                onEquipmentChangedCallback.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            UnEquip(i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            UnequipAll();
        }
    }
}
