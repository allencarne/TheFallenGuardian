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
    }

    public void LocalMultiPlayer()
    {
        OnLocalMultiplayerSelected.Invoke();

        gameModePanel.SetActive(false);

        localMultiplayerPanel.SetActive(true);
    }

    public void OnlineMultiPlayer()
    {
        OnOnlineMultiplayerSelected.Invoke();

        gameModePanel.SetActive(false);
    }

    public void Player1Panel()
    {
        playerPanel.SetActive(false);
        player1Panel.SetActive(false);
    }

    public void Player2Panel()
    {
        player2Panel.SetActive(false);
    }
}
