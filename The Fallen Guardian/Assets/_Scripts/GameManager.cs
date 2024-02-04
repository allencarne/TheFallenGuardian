using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    public UnityEvent OnPlayerJoin;
    public UnityEvent OnPlayer2Join;

    [Header("Players")]
    public GameObject playerInstance;
    public GameObject player2Instance;

    [Header("Components")]
    [SerializeField] SelectGameMode selectGameMode;
    [SerializeField] PlayerInputManager playerInputManager;
    public Camera player2Camera;

    public enum GameMode
    {
        None,
        Singleplayer,
        LocalMultiplayer,
        OnlineMultiplayer
    }

    public GameMode gameMode = GameMode.None;

    public void SinglePlayer()
    {
        playerInputManager.EnableJoining();
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInstance == null)
        {
            playerInstance = playerInput.gameObject;

            playerInstance.GetComponent<Player>().PlayerIndex = 1;

            OnPlayerJoin.Invoke();
        }
        else
        {
            player2Instance = playerInput.gameObject;

            player2Instance.GetComponent<Player>().PlayerIndex = 2;

            OnPlayer2Join.Invoke();
        }
    }


    public void LocalMultiplayer()
    {
        playerInputManager.EnableJoining();
    }

    public void OnlineMultiplayer()
    {
        // Implement OnlineMultiplayer logic
        Debug.Log("OnlineMultiplayer selected");
    }
}
