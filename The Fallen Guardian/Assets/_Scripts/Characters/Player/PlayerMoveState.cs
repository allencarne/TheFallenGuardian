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

        if (movement != Vector2.zero)
        {
            // Snap the movement direction
            Vector2 snappedDirection = stateMachine.SnapDirection(movement);

            stateMachine.BodyAnimator.SetFloat("Horizontal", snappedDirection.x);
            stateMachine.BodyAnimator.SetFloat("Vertical", snappedDirection.y);

            stateMachine.SwordAnimator.SetFloat("Horizontal", snappedDirection.x);
            stateMachine.SwordAnimator.SetFloat("Vertical", snappedDirection.y);
        }
    }
}
