using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    PlayerInputHandler inputHandler;
    Camera mainCamera;
    public bool isControllerInput;

    private void Awake()
    {
        inputHandler = GetComponentInParent<PlayerInputHandler>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (inputHandler != null)
        {
            Rotate();
        }
    }

    public void Rotate()
    {
        if (isControllerInput)
        {
            // Get the look input values
            float horizontalLook = inputHandler.LookInput.x;
            float verticalLook = inputHandler.LookInput.y;

            // Calculate the rotation angle based on input
            float angle = Mathf.Atan2(verticalLook, horizontalLook) * Mathf.Rad2Deg;

            // Apply rotation to the Aimer
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));

            // Calculate the rotation angle based on mouse position
            float angle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg;

            // Apply rotation to the Aimer
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
