using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    PlayerInputHandler inputHandler;
    Rigidbody2D rb;

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
                HandleMovement(inputHandler.MoveInput);
                break;
            case PlayerState.Move:

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

    }

    void MoveState()
    {

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
