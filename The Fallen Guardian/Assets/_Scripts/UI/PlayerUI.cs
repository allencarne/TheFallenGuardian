using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObjectRuntimeSet playerUIReference;
    GameObject player1UI;
    GameObject player2UI;

    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject inventory2UI;

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
            inventory2UI = player2UI.transform.Find("Inventory Parent").transform.Find("Inventory").gameObject;

            inventory2UI.SetActive(!inventory2UI.activeSelf);
        }


        /*
        GameObject player2UI = Camera.main.GetComponent<CameraFollow>().player2UI;

        if (Camera.main.GetComponent<CameraFollow>().player2UI)
        {
            // Get "Inventory" child game object from the Player2UI
            inventory2UI = player2UI.transform.Find("Inventory Parent").transform.Find("Inventory").gameObject;

            inventory2UI.SetActive(!inventory2UI.activeSelf);
        }
        */
    }
}
