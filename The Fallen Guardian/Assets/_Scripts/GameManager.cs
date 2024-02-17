using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerReference;
    [SerializeField] List<CharacterStats> characterStatsList;

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
        Character character = playerInput.gameObject.GetComponent<Character>();
        Player player = playerInput.gameObject.GetComponent<Player>();

        if (playerReference.items.Count == 0)
        {
            // Event for Handling the SelectGameMode UI
            OnPlayerJoin.Invoke();

            // Assign Player Index for Camrea Tracking
            player.PlayerIndex = 1;

            // Create a New ScriptableObject PlayerStats - Assigns Default Values in Method
            CharacterStats newCharacterStats = CreateNewCharacterStats();

            // Add New ScriptableObject to List
            characterStatsList.Add(newCharacterStats);

            // Assign ScriptableObject to Instantiated Player
            character.characterStats = newCharacterStats;
        }
        else
        {
            // Event for Handling the SelectGameMode UI
            OnPlayer2Join.Invoke();

            // Assign Player Index for Camrea Tracking
            player.PlayerIndex = 2;

            // Create a New ScriptableObject PlayerStats - Assigns Default Values in Method
            CharacterStats newPlayer2Stats = CreateNewCharacterStats();

            // Add New ScriptableObject to List
            characterStatsList.Add(newPlayer2Stats);

            // Assign ScriptableObject to Instantiated Player
            character.characterStats = newPlayer2Stats;

            // Spawn Second Camera
            Instantiate(player2CameraPrefab);
        }
    }

    // Method to create a new PlayerStats asset
    CharacterStats CreateNewCharacterStats()
    {
        CharacterStats newCharacterStats = ScriptableObject.CreateInstance<CharacterStats>();
        newCharacterStats.health = 10f;
        newCharacterStats.maxHealth = 10f;
        newCharacterStats.movementSpeed = 8f;
        newCharacterStats.damage = 1f; // Temp, will be 0
        newCharacterStats.playerClass = PlayerClass.Beginner;
        return newCharacterStats;
    }
}
