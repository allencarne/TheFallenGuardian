using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject mapMenu;
    [SerializeField] GameObject statsMenu;
    [SerializeField] GameObject skillMenu;
    [SerializeField] GameObject inventoryUI;

    public void OnSettingsPressed()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    public void OnMapPressed()
    {
        mapMenu.SetActive(!mapMenu.activeSelf);
    }

    public void OnStatsPressed()
    {
        statsMenu.SetActive(!statsMenu.activeSelf);
    }

    public void OnSkillsPressed()
    {
        skillMenu.SetActive(!skillMenu.activeSelf);
    }

    public void OnInventoryPressed()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }
}
