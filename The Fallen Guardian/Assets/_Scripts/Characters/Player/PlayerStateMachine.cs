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
    [SerializeField] Animator headAnimator; public Animator HeadAnimator => headAnimator;
    [SerializeField] Animator chestAnimator; public Animator ChestAnimator => chestAnimator;
    [SerializeField] Animator legsAnimator; public Animator LegsAnimator => legsAnimator;
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
        /*
        PlayerInput controlScheme = GetComponent<PlayerInput>();

        if (controlScheme.currentControlScheme == "Keyboard")
        {
            float distance = Vector3.Distance(transform.position, inputHandler.MousePosition);
            if (distance > slideRange)
            {
                canSlideForward = true;
            }
        }
        */
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
    public Vector2 HandleDirection(float angle)
    {
        Vector2 direction;

        if (angle >= 45 && angle < 135) // Up
        {
            direction = new Vector2(0, 1);
        }
        else if (angle >= 135 && angle < 225) // Left
        {
            direction = new Vector2(-1, 0);
        }
        else if (angle >= 225 && angle < 315) // Down
        {
            direction = new Vector2(0, -1);
        }
        else // Right (covers 315 to 44.999 degrees)
        {
            direction = new Vector2(1, 0);
        }

        return direction;
    }

    public Vector2 SnapDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0;
            direction.x = Mathf.Sign(direction.x);
        }
        else
        {
            direction.x = 0;
            direction.y = Mathf.Sign(direction.y);
        }

        return direction;
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