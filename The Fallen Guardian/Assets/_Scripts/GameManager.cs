using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player1Instance;
    [SerializeField] GameObject player2Instance;

    PlayerInputManager playerInputManager;

    enum GameMode
    {
        Singleplayer,
        LocalMultiplayer,
        OnlineMultiplayer
    }

    GameMode gameMode;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        Debug.Log(gameMode);

        switch (PlayerPrefs.GetInt("SelectedGameMode"))
        {
            case 0:
                gameMode = GameMode.Singleplayer;
                break;
            case 1:
                gameMode = GameMode.LocalMultiplayer;
                break;
            case 2:
                gameMode = GameMode.OnlineMultiplayer;
                break;
        }
    }
}
