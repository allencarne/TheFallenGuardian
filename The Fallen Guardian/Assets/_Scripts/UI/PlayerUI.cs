using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] GameObject inventoryUI;
    [SerializeField] Image inventoryHighlight;

    [Header("Stats")]
    [SerializeField] GameObject statsUI;
    [SerializeField] Image statsHighlight;

    [Header("Ability")]
    [SerializeField] GameObject AbilityUI;
    [SerializeField] Image abilityHighlight;

    [Header("Pause")]
    [SerializeField] GameObject pauseMenu;

    [Header("Events")]
    public UnityEvent OnStatsUI;
    public UnityEvent OnInventoryUI;
    public UnityEvent OnAbilityUI;

    public void OnInventoryUIOpened()
    {
        if (inventoryHighlight.enabled == true)
        {
            inventoryHighlight.enabled = false;
        }

        inventoryUI.SetActive(!inventoryUI.activeSelf);

        OnInventoryUI?.Invoke();
    }

    public void OnStatsUIOpened()
    {
        OnStatsUI?.Invoke();

        statsUI.SetActive(!statsUI.activeSelf);
    }

    public void OnAbilityUIOpened()
    {
        if (abilityHighlight.enabled == true)
        {
            abilityHighlight.enabled = false;
        }

        AbilityUI.SetActive(!AbilityUI.activeSelf);

        OnAbilityUI?.Invoke();
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

    public void OnQuestAccepted()
    {
        inventoryHighlight.enabled = true;
    }

    public void OnAbilityQuestAccepted()
    {
        abilityHighlight.enabled = true;
    }
}
