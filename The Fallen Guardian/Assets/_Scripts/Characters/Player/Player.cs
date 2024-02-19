using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public int PlayerIndex;
    public PlayerStats playerStats;

    /*
    public PlayerClass currentPlayerClass;

    public void SetPlayerClass(PlayerClass newClass)
    {
        currentPlayerClass = newClass;
    }

    public void UseBasicAttack()
    {
        if (currentPlayerClass != null)
        {
            currentPlayerClass.UseBasicAttack();
        }
    }

    [Header("Exposed Components")]
    [SerializeField] protected Animator bodyAnimator;
    [SerializeField] protected Transform aimer;

    [Header("Un-Exposed Components")]
    protected Rigidbody2D rb;
    protected PlayerInputHandler inputHandler;

    [Header("Exposed Variables")]
    [SerializeField] protected float SlideForwardSpeed;
    [SerializeField] float slideRange;
    [SerializeField] float slideDuration;

    [Header("Un-Exposed Variables")]
    protected bool canSpawn = true;
    protected bool canBasicAttack = true;
    protected bool canSlideForward = false;

    [SerializeField] protected Quaternion attackDir;

    bool canHurt = true;

    [Header("Beginner")]
    [SerializeField] GameObject Club;
    [SerializeField] Animator clubAnimator;
    [SerializeField] bool isClubEquipt;
    [SerializeField] GameObject clubSlash;

    public enum PlayerState
    {
        Spawn,
        Idle,
        Move,
        Hurt,
        BasicAttack,
        BasicAttack2,
        BasicAttack3,
        WarriorBasicAttack,
    }

    public PlayerState state = PlayerState.Idle;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        characterStats.health = characterStats.maxHealth;
    }

    private void Update()
    {
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
            if (PlayerIndex == 1)
            {
                characterStats.health -= 1;

                TakeDamage(1);
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

                if (canSlideForward)
                {
                    StartCoroutine(SlideDuration());

                    SlideForward(attackDir.eulerAngles.z);
                }

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
        bodyAnimator.Play("Idle");
        clubAnimator.Play("Idle");

        // Transitions
        HandleAttack(inputHandler.BasicAttackInput);
    }

    protected virtual void MoveState()
    {
        clubAnimator.Play("Move");

        // Set idle Animation after move
        if (inputHandler.MoveInput != Vector2.zero)
        {
            clubAnimator.SetFloat("Horizontal", inputHandler.MoveInput.x);
            clubAnimator.SetFloat("Vertical", inputHandler.MoveInput.y);
        }

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
        Vector2 movement = moveInput.normalized * characterStats.movementSpeed;
        rb.velocity = movement;
    }

    protected void HandleSlideForward(float rotation)
    {
        PlayerInput controlScheme = GetComponent<PlayerInput>();

        if (controlScheme.currentControlScheme == "Keyboard")
        {
            // Calculate the distance between the player and the mouse position
            float distance = Vector3.Distance(transform.position, inputHandler.MousePosition);

            // Check if the distance is greater than the slide range
            if (distance > slideRange)
            {
                canSlideForward = true;
            }
        }

        // Slide If Movement Key/button is pressed
        if (inputHandler.MoveInput != Vector2.zero)
        {
            canSlideForward = true;
        }
    }

    protected void SlideForward(float rotation)
    {
        // Use the rotation of the Aimer to determine the slide direction
        Vector2 slideDirection = Quaternion.Euler(0f, 0f, rotation) * Vector2.right;

        rb.velocity = slideDirection * SlideForwardSpeed;
    }

    IEnumerator SlideDuration()
    {
        yield return new WaitForSeconds(slideDuration);

        canSlideForward = false;
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
            clubAnimator.Play("Hurt");

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
        if (canBasicAttack)
        {
            attackDir = aimer.rotation;

            canBasicAttack = false;

            bodyAnimator.Play("Sword Basic Attack");
            clubAnimator.Play("Basic Attack");

            FaceAttackingDirection(bodyAnimator);
            FaceAttackingDirection(clubAnimator);

            StartCoroutine(AttackImpact());
            StartCoroutine(DurationOfBasicAttack());
        }
    }

    IEnumerator AttackImpact()
    {
        yield return new WaitForSeconds(.3f);

        if (state == PlayerState.BasicAttack)
        {
            GameObject slash = Instantiate(clubSlash, transform.position, attackDir);
            Physics2D.IgnoreCollision(slash.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
            slash.GetComponent<DamageOnTrigger>().playerDamage = 1;
            slash.GetComponent<DamageOnTrigger>().abilityDamage = 1;

            HandleSlideForward(attackDir.eulerAngles.z);
        }
    }

    IEnumerator DurationOfBasicAttack()
    {
        yield return new WaitForSeconds(.8f);

        // If we are interrupted this should prevent issues with state changes
        if (state == PlayerState.BasicAttack)
        {
            state = PlayerState.Idle;
        }

        canBasicAttack = true;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slideRange);
    }
    */
}
