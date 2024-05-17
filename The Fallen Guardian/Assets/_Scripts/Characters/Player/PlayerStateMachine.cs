using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerState state;

    [Header("Components")]
    [SerializeField] Animator swordAnimator; public Animator SwordAnimator => swordAnimator;
    [SerializeField] Animator bodyAnimator; public Animator BodyAnimator => bodyAnimator;
    [SerializeField] Rigidbody2D rb; public Rigidbody2D Rigidbody => rb;
    [SerializeField] Transform aimer; public Transform Aimer => aimer;

    [Header("Scripts")]
    [SerializeField] PlayerInputHandler inputHandler; public PlayerInputHandler InputHandler => inputHandler;
    [SerializeField] PlayerEquipment equipment; public PlayerEquipment Equipment => equipment;
    [SerializeField] PlayerAbilities abilities; public PlayerAbilities Abilities => abilities;
    [SerializeField] Player player; public Player Player => player;

    // Variables
    public Vector2 LastMoveDirection = Vector2.zero;

    [Header("Slide")]
    bool canSlideForward = false;
    bool isSliding = false;
    float slideForce;
    float slideDuration;

    [Header("Basic Ability")]
    [HideInInspector] public bool CanBasicAbility = true;
    [HideInInspector] public Quaternion AbilityDir;
    [HideInInspector] public bool hasAttacked = false;

    [Header("Offensive Ability")]
    [HideInInspector] public bool canOffensiveAbility = true;

    [Header("Mobility Ability")]
    [HideInInspector] public bool canMobilityAbility = true;

    private void Awake()
    {
        SetState(new PlayerSpawnState(this));
    }

    void Update()
    {
        state.Update();
    }

    public void OnPlayerDeath()
    {
        SetState(new PlayerDeathState(this));
    }

    private void FixedUpdate()
    {
        state.FixedUpdate();

        if (canSlideForward && !isSliding)
        {
            StartCoroutine(SlideDuration(AbilityDir.eulerAngles.z));
        }
    }

    public void SetState(PlayerState newState) => state = newState;

    public void BasicAbility(bool abilityInput)
    {
        if (abilityInput && CanBasicAbility && Equipment.IsWeaponEquipt && abilities.basicAbilityReference != null)
        {
            if (!Player.CrowdControl.IsDisarmed)
            {
                player.PlayerEnterCombat();

                SetState(new PlayerBasicState(this));
            }
        }
    }

    public void OffensiveAbility(bool abilityInput)
    {
        if (abilityInput && canOffensiveAbility && Equipment.IsWeaponEquipt && abilities.offensiveAbilityReference != null)
        {
            player.PlayerEnterCombat();

            SetState(new PlayerOffensiveState(this));
        }
    }

    public void MobilityAbility(bool abilityInput)
    {
        if (abilityInput && canMobilityAbility && Equipment.IsWeaponEquipt && abilities.mobilityAbilityReference != null)
        {
            player.PlayerEnterCombat();

            SetState(new PlayerMobilityState(this));
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
            float distance = Vector3.Distance(transform.position, inputHandler.MousePosition);
            if (distance > slideRange)
            {
                canSlideForward = true;
            }
        }

        if (inputHandler.MoveInput != Vector2.zero)
        {
            canSlideForward = true;
        }
    }

    protected void SlideForward(float rotation)
    {
        Vector2 slideDirection = Quaternion.Euler(0f, 0f, rotation) * Vector2.right;
        rb.velocity = slideDirection * slideForce;
    }

    IEnumerator SlideDuration(float rotation)
    {
        isSliding = true; // Set the flag to true when the Coroutine starts
        float elapsedTime = 0f;
        float initialSlideForce = slideForce;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.fixedDeltaTime;
            slideForce = Mathf.Lerp(initialSlideForce, 0, elapsedTime / slideDuration);
            SlideForward(rotation);
            yield return new WaitForFixedUpdate();
        }

        slideForce = 0;
        canSlideForward = false;
        isSliding = false; // Set the flag back to false when the Coroutine completes
    }

    #endregion
    /*
    public void HandleAnimation(Animator animator, string type, string state, Vector2 direction)
    {
        string animationName = $"{type}_{state}";

        if (direction != Vector2.zero)
        {
            // Update last move direction
            LastMoveDirection = direction.normalized;

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
            if (LastMoveDirection != Vector2.zero)
            {
                // Determine idle animation direction based on last move direction
                if (Mathf.Abs(LastMoveDirection.x) > Mathf.Abs(LastMoveDirection.y))
                {
                    // Last move was horizontal
                    animator.Play($"{animationName}_{(LastMoveDirection.x > 0 ? "Right" : "Left")}");
                }
                else
                {
                    // Last move was vertical
                    animator.Play($"{animationName}_{(LastMoveDirection.y > 0 ? "Up" : "Down")}");
                }
            }
            else
            {
                // Character is idle and last move direction is zero, play default idle animation
                animator.Play($"{animationName}_Down");
            }
        }
    }
    */

    public void HandleAnimation(Animator animator, string animationName, Vector2 direction)
    {
        // Update last move direction
        LastMoveDirection = direction.normalized;

        if (direction != Vector2.zero)
        {
            // Character is moving or attacking
            animator.Play(animationName);
        }
        else
        {
            // Character is not moving or attacking, play idle animation
            if (LastMoveDirection != Vector2.zero)
            {
                // Determine idle animation direction based on last move direction
                if (Mathf.Abs(LastMoveDirection.x) > Mathf.Abs(LastMoveDirection.y))
                {
                    // Last move was horizontal
                    animator.Play($"{animationName}_{(LastMoveDirection.x > 0 ? "Right" : "Left")}");
                }
                else
                {
                    // Last move was vertical
                    animator.Play($"{animationName}_{(LastMoveDirection.y > 0 ? "Up" : "Down")}");
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