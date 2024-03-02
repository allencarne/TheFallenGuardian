using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObjectRuntimeSet playerInventoryReference;
    GameObject player1Inventory;
    GameObject player2Inventory;

    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject inventory2UI;

    private void Update()
    {
        Debug.Log(playerInventoryReference.items.Count);

        if (playerInventoryReference.items.Count > 0)
        {
            player1Inventory = playerInventoryReference.GetItemIndex(0).gameObject;
        }
    }

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
            inventory2UI = player2UI.transform.Find("Inventory Parent").transform.Find("Inventory").gameObject;

            inventory2UI.SetActive(!inventory2UI.activeSelf);
        }
    }
}
