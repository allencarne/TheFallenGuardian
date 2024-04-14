using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject statsUI;
    [SerializeField] GameObject AbilityUI;

    public UnityEvent OnStatsUI;

    public void OnInventoryUIOpened()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    public void OnStatsUIOpened()
    {
        OnStatsUI?.Invoke();

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
