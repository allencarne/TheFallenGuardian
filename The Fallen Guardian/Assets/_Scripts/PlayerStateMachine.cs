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

    public void HandleAnimation(Animator animator, string type, string state)
    {
        string animationName = $"{type}_{state}";

        if (InputHandler.MoveInput != Vector2.zero)
        {
            // Character is moving
            lastMoveDirection = InputHandler.MoveInput.normalized;

            // Determine animation direction based on current move input
            if (Mathf.Abs(InputHandler.MoveInput.x) > Mathf.Abs(InputHandler.MoveInput.y))
            {
                // Moving horizontally
                if (InputHandler.MoveInput.x > 0)
                {
                    // Moving right
                    animator.Play($"{animationName}_Right");
                }
                else
                {
                    // Moving left
                    animator.Play($"{animationName}_Left");
                }
            }
            else
            {
                // Moving vertically
                if (InputHandler.MoveInput.y > 0)
                {
                    // Moving up
                    animator.Play($"{animationName}_Up");
                }
                else
                {
                    // Moving down
                    animator.Play($"{animationName}_Down");
                }
            }
        }
        else
        {
            // Character is not moving, play idle animation based on last move direction
            if (lastMoveDirection != Vector2.zero)
            {
                // Determine idle animation direction based on last move direction
                if (Mathf.Abs(lastMoveDirection.x) > Mathf.Abs(lastMoveDirection.y))
                {
                    // Last move was horizontal
                    if (lastMoveDirection.x > 0)
                    {
                        // Last move was right
                        animator.Play($"{animationName}_Right");
                    }
                    else
                    {
                        // Last move was left
                        animator.Play($"{animationName}_Left");
                    }
                }
                else
                {
                    // Last move was vertical
                    if (lastMoveDirection.y > 0)
                    {
                        // Last move was up
                        animator.Play($"{animationName}_Up");
                    }
                    else
                    {
                        // Last move was down
                        animator.Play($"{animationName}_Down");
                    }
                }
            }
            else
            {
                // Character is idle and last move direction is zero, play default idle animation
                animator.Play($"{animationName}_Down");
            }
        }
    }
}