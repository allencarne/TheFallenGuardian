using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject statsUI;

    public void OnInventoryUIOpened()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void CloseInventoryButton()
    {
        inventoryUI.SetActive(false);
    }

    public void OpenStatsUIOpened()
    {
        statsUI.SetActive(!statsUI.activeSelf);
    }
}
