using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGameMode : MonoBehaviour
{
    [SerializeField] GameObject gameModePanel;

    [SerializeField] GameObject singlePlayerPanel;
    [SerializeField] GameObject localMultiplayerPanel;

    public event System.Action<int> OnGameModeSelected;

    private void Start()
    {
        gameModePanel.SetActive(true);

        singlePlayerPanel.SetActive(false);
        localMultiplayerPanel.SetActive(false);
    }

    public void SinglePlayer()
    {
        gameModePanel.SetActive(false);

        singlePlayerPanel.SetActive(true);

        OnGameModeSelected?.Invoke(1);
    }

    public void LocalMultiPlayer()
    {
        gameModePanel.SetActive(false);

        localMultiplayerPanel.SetActive(true);

        OnGameModeSelected?.Invoke(2);
    }

    public void OnlineMultiPlayer()
    {
        //gameModePanel.SetActive(false);

        //OnGameModeSelected?.Invoke(3);
    }
}
