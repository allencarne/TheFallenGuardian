using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("PlayerUI")]
    public GameObjectRuntimeSet playerUIReference;
    GameObject player1UI;
    GameObject player2UI;

    [Header("InventoryUI")]
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject inventory2UI;

    [Header("Inventory")]
    public Transform itemsParent;
    //public Transform equpmentParent;

    InventorySlot[] iSlots;
    //EquipmentSlot[] eSlots;

    private void Start()
    {
        iSlots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    public void OnPlayer1UICreated()
    {
        if (playerUIReference.items.Count > 0)
        {
            player1UI = playerUIReference.GetItemIndex(0).gameObject;
        }
    }

    public void OnPlayer2UICreated()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(1);

        player2UI = playerUIReference.GetItemIndex(1).gameObject;
    }

    public void OnInventoryUIOpened()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void OnInventory2UIOpened()
    {
        if (player2UI)
        {
            inventory2UI = player2UI.transform.Find("Inventory").gameObject;

            inventory2UI.SetActive(!inventory2UI.activeSelf);
        }
    }
}
