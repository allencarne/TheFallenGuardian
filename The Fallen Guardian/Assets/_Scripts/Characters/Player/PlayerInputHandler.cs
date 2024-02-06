using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool BasicAttackInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool OnInventoryInput { get; private set; }

    public GameObjectRuntimeSet cameraReference;
    Camera player2Camera;

    Camera player1Camera;
    Player player;

    private void Awake()
    {
        player1Camera = Camera.main;
        player = GetComponent<Player>();
    }

    private void Start()
    {
        if (cameraReference.items.Count > 0)
        {
            player2Camera = cameraReference.GetItemIndex(0).GetComponent<Camera>();
        }
    }

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

    public void OnMousePos(InputAction.CallbackContext context)
    {
        if (player.PlayerIndex == 1)
        {
            MousePosition = player1Camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
        }
        else
        {
            MousePosition = player2Camera.ScreenToWorldPoint(context.ReadValue<Vector2>());
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInventoryInput = true;
        }
    }
}
