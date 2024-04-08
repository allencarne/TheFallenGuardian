using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Update()
    {
        stateMachine.HandleAnimation(stateMachine.BodyAnimator, "Player", "Move", stateMachine.InputHandler.MoveInput.normalized);
        stateMachine.HandleAnimation(stateMachine.SwordAnimator, "Sword", "Move", stateMachine.InputHandler.MoveInput.normalized);

        // Transitions
        stateMachine.BasicAbility(stateMachine.InputHandler.BasicAbilityInput);
        stateMachine.OffensiveAbility(stateMachine.InputHandler.OffensiveAbilityInput);
        stateMachine.MobilityAbility(stateMachine.InputHandler.MobilityAbilityInput);
    }

    public override void FixedUpdate()
    {
        if (!stateMachine.CrowdControl.IsImmobilized)
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
        if (!stateMachine.Debuffs.IsSlowed)
        {
            Vector2 movement = moveInput.normalized * stateMachine.Player.Stats.Haste;
            stateMachine.Rigidbody.velocity = movement;
        }
        else
        {
            // Slowed behaviour
            Vector2 movement = moveInput.normalized * (stateMachine.Player.Stats.Haste - stateMachine.Debuffs.SlowAmount);
            stateMachine.Rigidbody.velocity = movement;
        }
    }
}
