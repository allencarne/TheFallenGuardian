using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than once instance of GameManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    [SerializeField] SelectGameMode selectGameMode;
    [SerializeField] PlayerInputManager playerInputManager;

    public GameObject playerInstance;
    public GameObject player2Instance;

    public enum GameMode
    {
        None,
        Singleplayer,
        LocalMultiplayer,
        OnlineMultiplayer
    }

    public GameMode gameMode = GameMode.None;

    private void OnEnable()
    {
        selectGameMode.OnGameModeSelected += HandleGameModeSelected;
    }

    private void OnDisable()
    {
        selectGameMode.OnGameModeSelected -= HandleGameModeSelected;
    }

    private void HandleGameModeSelected(int selectedMode)
    {
        // Set the game mode based on the integer value
        switch (selectedMode)
        {
            case 1:
                gameMode = GameMode.Singleplayer;
                SinglePlayer();
                break;
            case 2:
                gameMode = GameMode.LocalMultiplayer;
                LocalMultiplayer();
                break;
            case 3:
                gameMode = GameMode.OnlineMultiplayer;
                OnlineMultiplayer();
                break;
            default:
                Debug.LogWarning("Invalid game mode selected.");
                break;
        }
    }

    void SinglePlayer()
    {
        playerInputManager.EnableJoining();
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInstance == null)
        {
            playerInstance = playerInput.gameObject;
        }
        else
        {
            player2Instance = playerInput.gameObject;
        }
    }


    void LocalMultiplayer()
    {
        playerInputManager.EnableJoining();
    }

    void OnlineMultiplayer()
    {
        // Implement OnlineMultiplayer logic
        Debug.Log("OnlineMultiplayer selected");
    }
}
