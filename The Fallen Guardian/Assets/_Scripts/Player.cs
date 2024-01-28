using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [Header("Exposed Components")]
    [SerializeField] protected Animator bodyAnimator;
    [SerializeField] protected Transform aimer;

    [Header("Un-Exposed Components")]
    protected Rigidbody2D rb;
    protected PlayerInputHandler inputHandler;

    [Header("Exposed Variables")]
    [SerializeField] protected float SlideForwardSpeed;

    [Header("Un-Exposed Variables")]
    protected bool canBasicAttack = true;
    protected bool canSlideForward = false;

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
        Debug.Log(canSlideForward);

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

                SlideForward();

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

        // Transitions
        HandleAttack(inputHandler.BasicAttackInput);
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

        // Transitions
        HandleAttack(inputHandler.BasicAttackInput);
    }

    void HandleMovement(Vector2 moveInput)
    {
        Vector2 movement = moveInput.normalized * movementSpeed;
        rb.velocity = movement;
    }

    protected void SlideForward()
    {
        if (canSlideForward)
        {
            canSlideForward = false;

            // Use the rotation of the Aimer to determine the slide direction
            Vector2 slideDirection = Quaternion.Euler(0f, 0f, aimer.rotation.eulerAngles.z) * Vector2.right;

            rb.velocity = slideDirection * SlideForwardSpeed;
        }
    }

    protected void FaceAttackingDirection()
    {
        // Use Aimer rotation for setting animator parameters
        float angle = aimer.rotation.eulerAngles.z;

        // Convert angle to a normalized vector for animation parameters
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        bodyAnimator.SetFloat("Horizontal", direction.x);
        bodyAnimator.SetFloat("Vertical", direction.y);
    }

    protected virtual void BasicAttackState()
    {

    }

    protected virtual void BasicAttack2State()
    {

    }

    protected virtual void BasicAttack3State()
    {

    }

    void HandleAttack(bool attackInput)
    {
        if (attackInput && canBasicAttack)
        {
            // Prevents Unwanted Slide
            rb.velocity = Vector2.zero;

            state = PlayerState.BasicAttack;
        }
    }
}
