using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    bool isPlayer1 = true;
    [SerializeField] GameObjectRuntimeSet playerReference;

    [Header("Events")]
    public UnityEvent OnPlayerJoin;
    public UnityEvent OnPlayer2Join;

    [Header("Components")]
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
        }
        else
        {
            OnPlayer2Join.Invoke();
        }
        /*
        if (isPlayer1)
        {
            isPlayer1 = false;

            OnPlayerJoin.Invoke();
        }
        else
        {
            OnPlayer2Join.Invoke();
        }
        */
    }
}
