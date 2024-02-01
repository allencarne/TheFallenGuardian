using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGameMode : MonoBehaviour
{
    [SerializeField] GameObject gameModePanel;

    [SerializeField] GameObject singlePlayerPanel;
    [SerializeField] GameObject localMultiplayerPanel;

    private void Start()
    {
        gameModePanel.SetActive(true);

        singlePlayerPanel.SetActive(false);
        localMultiplayerPanel.SetActive(false);
    }

    public void SinglePlayer()
    {
        PlayerPrefs.SetInt("SelectedGameMode", 0);

        ModeSelected();
    }

    public void LocalMultiPlayer()
    {
        PlayerPrefs.SetInt("SelectedGameMode", 1);

        ModeSelected();
    }

    public void OnlineMultiPlayer()
    {
        PlayerPrefs.SetInt("SelectedGameMode", 2);

        ModeSelected();
    }

    void ModeSelected()
    {
        gameModePanel.SetActive(false);

        switch (PlayerPrefs.GetInt("SelectedGameMode"))
        {
            case 0:
                singlePlayerPanel.SetActive(true);
                break;
            case 1:
                localMultiplayerPanel.SetActive(true);
                break;
            case 2:
                // Online Mulitplayer
                break;
        }
    }
}
