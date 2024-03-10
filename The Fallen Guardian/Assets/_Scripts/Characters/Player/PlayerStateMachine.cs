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

    [SerializeField] PlayerEquipment equipment;

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

    [Header("Basic Ability")]
    public bool CanBasicAbility = true;
    public Quaternion AbilityDir;

    [Header("Offensive Ability")]
    public bool canOffensiveAbility = true;

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

            SlideForward(AbilityDir.eulerAngles.z);
        }
    }

    public void SetState(PlayerState newState) => state = newState;

    public void BasicAbility(bool abilityInput)
    {
        if (abilityInput && CanBasicAbility && equipment.IsWeaponEquipt && abilities.basicAbilityReference != null)
        {
            // Prevents Unwanted Slide
            rb.velocity = Vector2.zero;

            SetState(new PlayerBasicState(this));
        }
    }

    public void OffensiveAbility(bool abilityInput)
    {
        if (abilityInput && canOffensiveAbility && equipment.IsWeaponEquipt && abilities.offensiveAbilityReference != null)
        {
            // Prevents Unwanted Slide
            rb.velocity = Vector2.zero;

            SetState(new PlayerOffensiveState(this));
        }
    }

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