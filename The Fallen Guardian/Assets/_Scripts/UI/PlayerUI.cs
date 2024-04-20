using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject statsUI;
    [SerializeField] GameObject AbilityUI;
    [SerializeField] GameObject pauseMenu;

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

    public void OnPauseUIOpened()
    {
        // If inventory or Stats or Ability UI are open, Close them
        if (inventoryUI.activeInHierarchy || statsUI.activeInHierarchy || AbilityUI.activeInHierarchy)
        {
            inventoryUI.SetActive(false);
            statsUI.SetActive(false);
            AbilityUI.SetActive(false);

            return;
        }
        else
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(true);
            }
        }
    }
}
