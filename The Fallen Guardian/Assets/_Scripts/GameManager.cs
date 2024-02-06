using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerReference;

    [Header("Events")]
    public UnityEvent OnPlayerJoin;
    public UnityEvent OnPlayer2Join;

    [Header("Components")]
    [SerializeField] Camera player2CameraPrefab;
    [SerializeField] SelectGameMode selectGameMode;
    [SerializeField] PlayerInputManager playerInputManager;

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
        Debug.Log("OnlineMultiplayer selected");
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerReference.items.Count == 0)
        {
            OnPlayerJoin.Invoke();

            playerInput.gameObject.GetComponent<Player>().PlayerIndex = 1;
        }
        else
        {
            OnPlayer2Join.Invoke();

            playerInput.gameObject.GetComponent<Player>().PlayerIndex = 2;

            Instantiate(player2CameraPrefab);
        }
    }
}
