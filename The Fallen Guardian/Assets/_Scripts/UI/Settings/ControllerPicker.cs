using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerPicker : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnKeyboard();
        }

        if (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed)
        {
            OnGamePad();
        }
    }

    public void OnKeyboard()
    {
        gameManager.controllerType = GameManager.ControllerType.Keyboard;
        gameManager.OnPlayerJoined();
    }

    public void OnGamePad()
    {
        gameManager.controllerType = GameManager.ControllerType.Gamepad;
        gameManager.OnPlayerJoined();
    }

    public void OnTouch()
    {
        gameManager.controllerType = GameManager.ControllerType.Touch;
    }
}
