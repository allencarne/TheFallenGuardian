using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject statsUI;
    [SerializeField] GameObject AbilityUI;

    public void OnInventoryUIOpened()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void OnStatsUIOpened()
    {
        statsUI.SetActive(!statsUI.activeSelf);
    }

    public void OnAbilityUIOpened()
    {
        AbilityUI.SetActive(!AbilityUI.activeSelf);
    }

    public void CloseInventoryUIButton()
    {
        inventoryUI.SetActive(false);
    }

    public void CloseStatsUIButton()
    {
        statsUI.SetActive(false);
    }

    public void CloseAbilityUIButton()
    {
        AbilityUI.SetActive(false);
    }
}
