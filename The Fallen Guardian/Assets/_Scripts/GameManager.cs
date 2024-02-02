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

    public enum GameMode
    {
        None,
        Singleplayer,
        LocalMultiplayer,
        OnlineMultiplayer
    }

    public GameMode gameMode = GameMode.None;

    private void Start()
    {

    }

    private void Update()
    {
        Debug.Log(gameMode);
    }
}
