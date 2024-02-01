using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGameMode : MonoBehaviour
{
    [SerializeField] GameObject gameModePanel;

    private void Start()
    {
        gameModePanel.SetActive(true);
    }

    public void SinglePlayer()
    {
        PlayerPrefs.SetInt("SelectedGameMode", 0);

        gameModePanel.SetActive(false);
    }

    public void LocalMultiPlayer()
    {
        PlayerPrefs.SetInt("SelectedGameMode", 1);

        gameModePanel.SetActive(false);
    }

    public void OnlineMultiPlayer()
    {
        PlayerPrefs.SetInt("SelectedGameMode", 2);

        gameModePanel.SetActive(false);
    }
}
