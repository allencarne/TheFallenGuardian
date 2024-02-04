using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectGameMode : MonoBehaviour
{
    public UnityEvent OnSingleplayerSelected;
    public UnityEvent OnLocalMultiplayerSelected;
    public UnityEvent OnOnlineMultiplayerSelected;

    [SerializeField] GameObject gameModePanel;

    [SerializeField] GameObject singleplayerPanel;
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

        singleplayerPanel.SetActive(false);
        localMultiplayerPanel.SetActive(false);
    }

    public void Singleplayer()
    {
        OnSingleplayerSelected.Invoke();

        gameModePanel.SetActive(false);

        singleplayerPanel.SetActive(true);

        //OnGameModeSelected?.Invoke(1);
    }

    public void LocalMultiPlayer()
    {
        OnLocalMultiplayerSelected.Invoke();

        gameModePanel.SetActive(false);

        localMultiplayerPanel.SetActive(true);

        //OnGameModeSelected?.Invoke(2);
    }

    public void OnlineMultiPlayer()
    {
        OnOnlineMultiplayerSelected.Invoke();

        gameModePanel.SetActive(false);

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
