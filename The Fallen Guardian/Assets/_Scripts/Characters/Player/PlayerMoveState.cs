using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Update()
    {
        stateMachine.BodyAnimator.Play("Move");

        stateMachine.SwordAnimator.Play("Move");

        //stateMachine.HandleAnimation(stateMachine.BodyAnimator, "Player", "Move", stateMachine.InputHandler.MoveInput.normalized);
        //stateMachine.HandleAnimation(stateMachine.SwordAnimator, "Sword", "Move", stateMachine.InputHandler.MoveInput.normalized);

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
            stateMachine.BodyAnimator.SetFloat("Horizontal", movement.x);
            stateMachine.BodyAnimator.SetFloat("Vertical", movement.y);

            stateMachine.SwordAnimator.SetFloat("Horizontal", movement.x);
            stateMachine.SwordAnimator.SetFloat("Vertical", movement.y);
        }
    }
}
