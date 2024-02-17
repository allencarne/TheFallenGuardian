using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerReference;
    [SerializeField] List<PlayerStats> playerStatsList;

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
        Player player = playerInput.gameObject.GetComponent<Player>();

        if (playerReference.items.Count == 0)
        {
            OnPlayerJoin.Invoke();

            player.PlayerIndex = 1;

            player.playerStats = playerStatsList[playerReference.items.Count];
        }
        else
        {
            OnPlayer2Join.Invoke();

            player.PlayerIndex = 2;

            player.playerStats = playerStatsList[playerReference.items.Count];

            Instantiate(player2CameraPrefab);
        }
    }
}
