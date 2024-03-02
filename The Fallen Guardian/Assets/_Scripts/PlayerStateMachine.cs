using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState state;

    [Header("Components")]
    [SerializeField] Player player;
    public Player Player => player;

    [SerializeField] Rigidbody2D rb;
    public Rigidbody2D Rigidbody => rb;

    [SerializeField] Animator bodyAnimator;
    public Animator BodyAnimator => bodyAnimator;

    [SerializeField] Animator clubAnimator;
    public Animator ClubAnimator => clubAnimator;

    [SerializeField] Animator swordAnimator;
    public Animator SwordAnimator => swordAnimator;

    [SerializeField] PlayerInputHandler inputHandler;
    public PlayerInputHandler InputHandler => inputHandler;

    [SerializeField] PlayerAbilities abilities;
    public PlayerAbilities Abilities => abilities;

    [SerializeField] Transform aimer;
    public Transform Aimer => aimer;

    private Vector2 lastMoveDirection = Vector2.zero;
    private Vector2 lastAttackDirection = Vector2.zero;

    [Header("Slide")]
    bool canSlideForward = false;
    float slideForce;
    float slideDuration;

    [Header("Basic Attack")]
    public bool CanBasicAttack = true;
    public Quaternion AttackDir;

    private void Awake()
    {
        SetState(new PlayerSpawnState(this));
    }

    void Update()
    {
        state.Update();
    }

    private void FixedUpdate()
    {
        state.FixedUpdate();

        if (canSlideForward)
        {
            StartCoroutine(SlideDuration());

            SlideForward(AttackDir.eulerAngles.z);
        }
    }

    public void SetState(PlayerState newState) => state = newState;

    #region Attack

    public void TransitionToBasicAttack(bool attackInput)
    {
        if (attackInput && CanBasicAttack)
        {
            // Prevents Unwanted Slide
            rb.velocity = Vector2.zero;

            SetState(new PlayerBasicAttackState(this));
        }
    }

    public void FaceAttackingDirection(Animator animator)
    {
        // Use Aimer rotation for setting animator parameters
        float angle = aimer.rotation.eulerAngles.z;

        // Convert angle to a normalized vector for animation parameters
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }

    #endregion

    #region SlideForward

    public void HandleSlideForward(float rotation, float slideRange, float nSlideForce, float nSlideDuration)
    {
        slideForce = nSlideForce;
        slideDuration = nSlideDuration;

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

        rb.velocity = slideDirection * slideForce;
    }

    IEnumerator SlideDuration()
    {
        yield return new WaitForSeconds(slideDuration);

        canSlideForward = false;
    }

    #endregion

    public void HandleAnimation(Animator animator, string type, string state, Vector2 direction)
    {
        string animationName = $"{type}_{state}";

        if (direction != Vector2.zero)
        {
            // Update last move direction
            lastMoveDirection = direction.normalized;

            // Character is moving or attacking
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Moving horizontally
                animator.Play($"{animationName}_{(direction.x > 0 ? "Right" : "Left")}");
            }
            else
            {
                // Moving vertically
                animator.Play($"{animationName}_{(direction.y > 0 ? "Up" : "Down")}");
            }
        }
        else
        {
            // Character is not moving or attacking, play idle animation
            if (lastMoveDirection != Vector2.zero)
            {
                // Determine idle animation direction based on last move direction
                if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
                {
                    // Last move was horizontal
                    animator.Play($"{animationName}_{(lastMoveDirection.x > 0 ? "Right" : "Left")}");
                }
                else
                {
                    // Last move was vertical
                    animator.Play($"{animationName}_{(lastMoveDirection.y > 0 ? "Up" : "Down")}");
                }
            }
            else
            {
                // Character is idle and last move direction is zero, play default idle animation
                animator.Play($"{animationName}_Down");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ItemPickup item = collision.GetComponent<ItemPickup>();
        if (item != null)
        {
            if (inputHandler.PickupInput)
            {
                item.PickUp();
            }
        }
    }
}