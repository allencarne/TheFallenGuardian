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

    [SerializeField] PlayerInputHandler inputHandler;
    public PlayerInputHandler InputHandler => inputHandler;

    [SerializeField] PlayerAbilities abilities;
    public PlayerAbilities Abilities => abilities;

    [SerializeField] Transform aimer;
    public Transform Aimer => aimer;


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
    }

    public void SetState(PlayerState newState) => state = newState;

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
    /*
    public  void HandleSlideForward(float rotation)
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
    */
}