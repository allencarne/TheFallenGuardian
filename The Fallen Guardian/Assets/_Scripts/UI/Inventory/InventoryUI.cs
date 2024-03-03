using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerInventoryReference;
    [SerializeField] Inventory inventory;

    [Header("Inventory")]
    public Transform itemsParent;
    //public Transform equpmentParent;

    InventorySlot[] iSlots;
    //EquipmentSlot[] eSlots;

    private void Start()
    {
        iSlots = itemsParent.GetComponentsInChildren<InventorySlot>();
        inventory = playerInventoryReference.GetItemIndex(0).GetComponent<Inventory>();
    }

    public void UpdateUI()
    {
        Debug.Log("yes");

        /*
        for (int i = 0; i < iSlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                iSlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                iSlots[i].ClearSlot();
            }
        }
        */
    }
}
