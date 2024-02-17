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
            // Event for Handling the SelectGameMode UI
            OnPlayerJoin.Invoke();

            // Assign Player Index for Camrea Tracking
            player.PlayerIndex = 1;

            // Create a New ScriptableObject PlayerStats - Assigns Default Values in Method
            PlayerStats newPlayerStats = CreateNewPlayerStats();

            // Add New ScriptableObject to List
            playerStatsList.Add(newPlayerStats);

            // Assign ScriptableObject to Instantiated Player
            player.playerStats = newPlayerStats;
        }
        else
        {
            // Event for Handling the SelectGameMode UI
            OnPlayer2Join.Invoke();

            // Assign Player Index for Camrea Tracking
            player.PlayerIndex = 2;

            // Create a New ScriptableObject PlayerStats - Assigns Default Values in Method
            PlayerStats newPlayer2Stats = CreateNewPlayerStats();

            // Add New ScriptableObject to List
            playerStatsList.Add(newPlayer2Stats);

            // Assign ScriptableObject to Instantiated Player
            player.playerStats = newPlayer2Stats;

            // Spawn Second Camera
            Instantiate(player2CameraPrefab);
        }
    }

    // Method to create a new PlayerStats asset
    PlayerStats CreateNewPlayerStats()
    {
        PlayerStats newPlayerStats = ScriptableObject.CreateInstance<PlayerStats>();
         newPlayerStats.health = 10f;
         newPlayerStats.maxHealth = 10f;
         newPlayerStats.movementSpeed = 8f;
         newPlayerStats.playerClass = PlayerClass.Beginner;
        return newPlayerStats;
    }
}
