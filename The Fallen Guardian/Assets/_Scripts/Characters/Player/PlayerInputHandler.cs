using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool BasicAbilityInput { get; private set; }
    public bool OffensiveAbilityInput { get; private set; }
    public bool MobilityAbilityInput { get; private set; }
    public bool DefensiveAbilityInput { get; private set; }
    public bool UtilityAbilityInput { get; private set; }
    public bool UltimateAbilityInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool PickupInput { get; private set; }

    public GameObjectRuntimeSet cameraReference;
    public Camera player2Camera;

    public Camera player1Camera;
    Player player;

    public UnityEvent OnPauseUI;
    public UnityEvent OnInventoryUI;
    public UnityEvent OnStatsUI;
    public UnityEvent OnAbilityUI;
    public UnityEvent OnMapUI;

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
    
    public void OnBasicAbility(InputAction.CallbackContext context)
    {
        if (!isMouseOverUI())
        {
            BasicAbilityInput = context.ReadValueAsButton();

            if (context.canceled)
            {
                BasicAbilityInput = false;
            }
        }
    }

    public void OnOffensiveAbility(InputAction.CallbackContext context)
    {
        if (!isMouseOverUI())
        {
            OffensiveAbilityInput = context.ReadValueAsButton();

            if (context.canceled)
            {
                OffensiveAbilityInput = false;
            }
        }
    }

    public void OnMobilityAbility(InputAction.CallbackContext context)
    {
        if (!isMouseOverUI())
        {
            MobilityAbilityInput = context.ReadValueAsButton();

            if (context.canceled)
            {
                MobilityAbilityInput = false;
            }
        }
    }

    public void OnDefensiveAbility(InputAction.CallbackContext context)
    {
        if (!isMouseOverUI())
        {
            DefensiveAbilityInput = context.ReadValueAsButton();

            if (context.canceled)
            {
                DefensiveAbilityInput = false;
            }
        }
    }

    public void OnUtilityAbility(InputAction.CallbackContext context)
    {
        if (!isMouseOverUI())
        {
            UtilityAbilityInput = context.ReadValueAsButton();

            if (context.canceled)
            {
                UtilityAbilityInput = false;
            }
        }
    }

    public void OnUltimateAbility(InputAction.CallbackContext context)
    {
        if (!isMouseOverUI())
        {
            UltimateAbilityInput = context.ReadValueAsButton();

            if (context.canceled)
            {
                UltimateAbilityInput = false;
            }
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

    public void OnPickup(InputAction.CallbackContext context)
    {
        PickupInput = context.ReadValueAsButton();

        if (context.canceled)
        {
            PickupInput = false;
        }
    }

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInventoryUI.Invoke();
        }
    }

    public void OnStatsInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnStatsUI.Invoke();
        }
    }

    public void OnAbilityInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnAbilityUI.Invoke();
        }
    }

    public void OnMapInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnMapUI.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPauseUI.Invoke();
        }
    }

    bool isMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}