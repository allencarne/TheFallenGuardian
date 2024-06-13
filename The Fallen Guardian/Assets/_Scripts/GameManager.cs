using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    GameObject playerInstance;

    public UnityEvent OnPlayerJoin;
    [SerializeField] GameObject playerSelectPanel;

    public enum ControllerType
    {
        None,
        Keyboard,
        Gamepad,
        Touch
    }

    public ControllerType controllerType;

    private void Start()
    {
        // Detect if we are playing on a mobile device
        if (Application.isMobilePlatform)
        {
            Debug.Log("test");
        }
        else
        {
            playerSelectPanel.SetActive(true);
        }
    }

    public void OnPlayerJoined()
    {
        playerInstance = Instantiate(playerPrefab);
        playerSelectPanel.SetActive(false);
        OnPlayerJoin.Invoke();
    }
}
