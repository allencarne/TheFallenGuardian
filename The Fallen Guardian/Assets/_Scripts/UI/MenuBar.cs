using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    [SerializeField] PlayerStats stats;

    [SerializeField] GameObject settingsMenu;

    [SerializeField] GameObject mapMenu;

    [SerializeField] GameObject statsMenu;

    [SerializeField] GameObject skillMenu;
    [SerializeField] Image image_skillHighlight;

    private void Start()
    {
        // Enable at the start so the player can select their first ability
        image_skillHighlight.enabled = true;
    }

    public void OnPlayerLevelUp()
    {
        if (stats.PlayerLevel == 5 && skillMenu.activeInHierarchy == false)
        {
            image_skillHighlight.enabled = true;
        }

        if (stats.PlayerLevel == 10 && skillMenu.activeInHierarchy == false)
        {
            image_skillHighlight.enabled = true;
        }
    }

    public void OnAbilityUIOpened()
    {
        if (image_skillHighlight.enabled == true)
        {
            image_skillHighlight.enabled = false;
        }
    }

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

        if (image_skillHighlight.enabled == true)
        {
            image_skillHighlight.enabled = false;
        }
    }
}
