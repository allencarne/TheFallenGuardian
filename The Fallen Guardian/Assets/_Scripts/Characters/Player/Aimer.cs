using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aimer : MonoBehaviour
{
    PlayerInputHandler inputHandler;
    public bool isControllerInput;
    PlayerInput playerInput;

    private void Awake()
    {
        inputHandler = GetComponentInParent<PlayerInputHandler>();
        playerInput = GetComponentInParent<PlayerInput>();
    }

    private void Update()
    {
        if (inputHandler != null && playerInput)
        {
            if (playerInput.currentControlScheme == "Gamepad")
            {
                RotateOnGamePad();
            }

            if (playerInput.currentControlScheme == "Keyboard")
            {
                RotateOnKeyboard();
            }
        }
    }

    void RotateOnKeyboard()
    {
        // Calculate the rotation angle based on mouse position
        float angle = Mathf.Atan2(inputHandler.MousePosition.y - transform.position.y, inputHandler.MousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        // Apply rotation to the Aimer
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void RotateOnGamePad()
    {
        // Get the look input values
        float horizontalLook = inputHandler.LookInput.x;
        float verticalLook = inputHandler.LookInput.y;

        // Calculate the rotation angle based on input
        float angle = Mathf.Atan2(verticalLook, horizontalLook) * Mathf.Rad2Deg;

        // Apply rotation to the Aimer
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
