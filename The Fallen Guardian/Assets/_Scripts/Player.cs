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
    protected bool canSpawn = true;
    protected bool canBasicAttack = true;
    protected bool canSlideForward = false;

    bool canHurt = true;

    public enum PlayerState
    {
        Spawn,
        Idle,
        Move,
        Hurt,
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
        //Debug.Log(state);

        switch (state)
        {
            case PlayerState.Spawn:
                SpawnState();
                break;
            case PlayerState.Idle:
                IdleState();
                break;
            case PlayerState.Move:
                MoveState();
                break;
            case PlayerState.Hurt:
                HurtState();
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isInterruptable)
            {
                state = PlayerState.Hurt;
            }
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

    protected virtual void SpawnState()
    {
        if (canSpawn)
        {
            canSpawn = false;

            StartCoroutine(SpawnDuration());
        }
    }

    IEnumerator SpawnDuration()
    {
        yield return new WaitForSeconds(.6f);

        state = PlayerState.Idle;

        canSpawn = true;
    }

    protected virtual void IdleState()
    {
        isInterruptable = true;

        bodyAnimator.Play("Idle");

        // Transitions
        HandleAttack(inputHandler.BasicAttackInput);
    }

    protected virtual void MoveState()
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

    protected void FaceAttackingDirection(Animator animator)
    {
        // Use Aimer rotation for setting animator parameters
        float angle = aimer.rotation.eulerAngles.z;

        // Convert angle to a normalized vector for animation parameters
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }

    protected virtual void HurtState()
    {
        if (canHurt)
        {
            canHurt = false;

            // Prevents Unwanted Slide
            rb.velocity = Vector2.zero;

            bodyAnimator.Play("Hurt");

            StartCoroutine(HurtDuration());
        }
    }

    IEnumerator HurtDuration()
    {
        yield return new WaitForSeconds(.8f);

        state = PlayerState.Idle;

        canHurt = true;
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
