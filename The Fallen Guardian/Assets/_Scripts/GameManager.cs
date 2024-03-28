using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnPlayerJoin;
    [SerializeField] GameObject playerSelectPanel;
    [SerializeField] PlayerInputManager playerInputManager;

    private void Start()
    {
        playerSelectPanel.SetActive(true);

        playerInputManager.EnableJoining();
    }

    void OnPlayerJoined()
    {
        playerSelectPanel.SetActive(false);

        OnPlayerJoin.Invoke();
    }
}
