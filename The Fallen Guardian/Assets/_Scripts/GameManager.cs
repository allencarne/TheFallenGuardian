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

    [Header("Events")]
    public UnityEvent OnPlayerJoin;
    public UnityEvent OnPlayer2Join;

    [Header("Players")]

    public GameObject playerInstance;
    //public GameObject player2Instance;

    [Header("Components")]
    [SerializeField] SelectGameMode selectGameMode;
    [SerializeField] PlayerInputManager playerInputManager;
    public Camera player2Camera;

    public void SinglePlayer()
    {
        playerInputManager.EnableJoining();
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

    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInstance == null)
        {
            //playerData.playerInstance = playerInput.gameObject;

            //playerInstance = playerInput.gameObject;

            //playerData.playerInstance.GetComponent<Player>().PlayerIndex = 1;

            OnPlayerJoin.Invoke();
        }
    }
}
