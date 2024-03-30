using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Debug.Log("UPDATING UI");
        for (var i = 0; i < iSlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                iSlots[i].AddItem(inventory.items.ElementAt(i).Key);
                iSlots[i].amount.text = inventory.items.ElementAt(i).Value.ToString();
            }
            else
            {
                iSlots[i].ClearSlot();
                iSlots[i].amount.text = "";
            }
        }
    }
}
