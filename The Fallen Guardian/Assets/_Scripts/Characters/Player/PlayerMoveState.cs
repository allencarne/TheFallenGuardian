using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Update()
    {
        stateMachine.BodyAnimator.Play("Move");
        stateMachine.SwordAnimator.Play("Move");

        // Transitions
        stateMachine.BasicAbility(stateMachine.InputHandler.BasicAbilityInput);
        stateMachine.OffensiveAbility(stateMachine.InputHandler.OffensiveAbilityInput);
        stateMachine.MobilityAbility(stateMachine.InputHandler.MobilityAbilityInput);
    }

    public override void FixedUpdate()
    {
        if (!stateMachine.Player.CrowdControl.IsImmobilized)
        {
            HandleMovement(stateMachine.InputHandler.MoveInput);
        }

        // If we are no longer moving - Transition to Idle State
        if (stateMachine.InputHandler.MoveInput == Vector2.zero)
        {
            stateMachine.SetState(new PlayerIdleState(stateMachine));
        }
    }

    void HandleMovement(Vector2 moveInput)
    {
        Vector2 movement = moveInput.normalized * stateMachine.Player.Stats.CurrentSpeed;
        stateMachine.Rigidbody.velocity = movement;

        // Determine the direction based on the angle of movement
        float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
        Vector2 direction = stateMachine.HandleDirection(angle);

        Debug.Log(direction);

        if (movement != Vector2.zero)
        {
            stateMachine.BodyAnimator.SetFloat("Horizontal", movement.x);
            stateMachine.BodyAnimator.SetFloat("Vertical", movement.y);

            stateMachine.SwordAnimator.SetFloat("Horizontal", movement.x);
            stateMachine.SwordAnimator.SetFloat("Vertical", movement.y);
        }
    }
}
