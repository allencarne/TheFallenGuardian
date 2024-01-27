using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [SerializeField] Animator bodyAnimator;
    Rigidbody2D rb;
    PlayerInputHandler inputHandler;

    public enum PlayerState
    {
        Idle,
        Move,
        BasicAttack,
        BasicAttack2,
        BasicAttack3,
    }

    public PlayerState state = PlayerState.Idle;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(state);

        switch (state)
        {
            case PlayerState.Idle:
                IdleState();
                break;
            case PlayerState.Move:
                MoveState();
                break;
            case PlayerState.BasicAttack:
                BasicAttackState();
                break;
            case PlayerState.BasicAttack2:
                BasicAttack2State();
                break;
            case PlayerState.BasicAttack3:
                BasicAttack3State();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case PlayerState.Idle:

                // Transition to Move State
                if (inputHandler.MoveInput != Vector2.zero)
                {
                    state = PlayerState.Move;
                }

                break;
            case PlayerState.Move:

                HandleMovement(inputHandler.MoveInput);

                // Tansition to Idle State
                if (inputHandler.MoveInput == Vector2.zero)
                {
                    state = PlayerState.Idle;
                }

                break;
            case PlayerState.BasicAttack:

                break;
            case PlayerState.BasicAttack2:

                break;
            case PlayerState.BasicAttack3:

                break;
        }
    }

    void IdleState()
    {
        bodyAnimator.Play("Idle");
    }

    void MoveState()
    {
        bodyAnimator.Play("Move");

        // Set idle Animation after move
        if (inputHandler.MoveInput != Vector2.zero)
        {
            bodyAnimator.SetFloat("Horizontal", inputHandler.MoveInput.x);
            bodyAnimator.SetFloat("Vertical", inputHandler.MoveInput.y);
        }
        bodyAnimator.SetFloat("Speed", inputHandler.MoveInput.sqrMagnitude);
    }

    void BasicAttackState()
    {

    }

    void BasicAttack2State()
    {

    }

    void BasicAttack3State()
    {

    }

    void HandleMovement(Vector2 moveInput)
    {
        Vector2 movement = moveInput.normalized * movementSpeed;
        rb.velocity = movement;
    }
}
