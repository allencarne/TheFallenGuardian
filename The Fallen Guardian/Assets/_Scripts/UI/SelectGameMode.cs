using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGameMode : MonoBehaviour
{
    [SerializeField] GameObject gameModePanel;

    [SerializeField] GameObject singlePlayerPanel;
    [SerializeField] GameObject localMultiplayerPanel;

    [SerializeField] GameObject playerPanel;

    [SerializeField] GameObject player1Panel;
    [SerializeField] GameObject player2Panel;

    public event System.Action<int> OnGameModeSelected;

    private void OnEnable()
    {
        GameManager.instance.OnPlayerJoin += PlayerPanel;
        GameManager.instance.OnPlayer2Join += Player2Panel;
    }

    private void OnDisable()
    {
        GameManager.instance.OnPlayerJoin -= PlayerPanel;
        GameManager.instance.OnPlayer2Join -= Player2Panel;
    }

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

    public void PlayerPanel()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Singleplayer)
        {
            playerPanel.SetActive(false);
        }

        if (GameManager.instance.gameMode == GameManager.GameMode.LocalMultiplayer)
        {
            player1Panel.SetActive(false);
        }
    }

    public void Player2Panel()
    {
        player2Panel.SetActive(false);
    }
}
