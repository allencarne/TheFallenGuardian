using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject inventory2UI;

    public void OnInventoryUIOpened()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void OnInventory2UIOpened()
    {
        GameObject player2UI = Camera.main.GetComponent<CameraFollow>().player2UI;

        if (Camera.main.GetComponent<CameraFollow>().player2UI)
        {
            // Get "Inventory" child game object from the Player2UI
            inventory2UI = player2UI.transform.Find("Inventory").gameObject;

            inventory2UI.SetActive(!inventory2UI.activeSelf);
        }
    }
}
