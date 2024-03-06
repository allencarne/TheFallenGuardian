using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    [Header("Inventory")]
    public Transform itemsParent;

    InventorySlot[] iSlots;

    private void Start()
    {
        inventory.onItemChangedCallback += UpdateUI;

        iSlots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void UpdateUI()
    {
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
    }
}
