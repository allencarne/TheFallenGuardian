using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool BasicAttackInput { get; private set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            MoveInput = Vector2.zero;
        }
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            LookInput = Vector2.zero;
        }
    }
    
    public void OnBasicAttack(InputAction.CallbackContext context)
    {
        BasicAttackInput = context.ReadValueAsButton();

        if (context.canceled)
        {
            BasicAttackInput = false;
        }
    }
}
