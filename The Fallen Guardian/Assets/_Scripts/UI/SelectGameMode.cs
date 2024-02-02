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
        GameManager.instance.gameMode = GameManager.GameMode.Singleplayer;

        gameModePanel.SetActive(false);

        singlePlayerPanel.SetActive(true);
    }

    public void LocalMultiPlayer()
    {
        GameManager.instance.gameMode = GameManager.GameMode.LocalMultiplayer;

        gameModePanel.SetActive(false);

        localMultiplayerPanel.SetActive(true);
    }

    public void OnlineMultiPlayer()
    {
        GameManager.instance.gameMode = GameManager.GameMode.OnlineMultiplayer;

        //gameModePanel.SetActive(false);
    }
}
