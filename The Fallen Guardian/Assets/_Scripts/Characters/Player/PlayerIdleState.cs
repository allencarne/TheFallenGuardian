using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        // State Logic Here
        stateMachine.HandleAnimation(stateMachine.BodyAnimator, "Player", "Idle", stateMachine.InputHandler.MoveInput.normalized);
        stateMachine.HandleAnimation(stateMachine.SwordAnimator, "Sword", "Idle", stateMachine.InputHandler.MoveInput.normalized);

        // Transitions
        stateMachine.BasicAbility(stateMachine.InputHandler.BasicAbilityInput);
        stateMachine.OffensiveAbility(stateMachine.InputHandler.OffensiveAbilityInput);
        stateMachine.MobilityAbility(stateMachine.InputHandler.MobilityAbilityInput);
    }

    public override void FixedUpdate()
    {
        // Transition to Move State
        if (stateMachine.InputHandler.MoveInput != Vector2.zero)
        {
            stateMachine.SetState(new PlayerMoveState(stateMachine));
        }
    }
}
