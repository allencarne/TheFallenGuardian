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
    //public UnityEvent OnPlayer2Join;

    [Header("Components")]
    [SerializeField] GameObject playerSelectPanel;
    //[SerializeField] Camera player2CameraPrefab;
    //[SerializeField] SelectGameMode selectGameMode;
    [SerializeField] PlayerInputManager playerInputManager;

    private void Start()
    {
        playerSelectPanel.SetActive(true);

        playerInputManager.EnableJoining();
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        playerSelectPanel.SetActive(false);

        Player player = playerInput.gameObject.GetComponent<Player>();
        HealthBar healthBar = player.GetComponent<HealthBar>();

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

            // Assign stats to healthbar
            healthBar.stats = player.playerStats;
        }
    }

    // Method to create a new PlayerStats asset
    PlayerStats CreateNewPlayerStats()
    {
        PlayerStats newPlayerStats = ScriptableObject.CreateInstance<PlayerStats>();
        newPlayerStats.health = 10;
        newPlayerStats.maxHealth = 10;
        newPlayerStats.movementSpeed = 8f;
        newPlayerStats.damage = 1; // Temp, will be 0
        newPlayerStats.playerClass = PlayerClass.Beginner;
        return newPlayerStats;
    }
}
