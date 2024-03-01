using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Update()
    {
        // State Logic Here
        stateMachine.HandleAnimation(stateMachine.BodyAnimator, "Player", "Idle");
        stateMachine.HandleAnimation(stateMachine.SwordAnimator, "Sword", "Idle");

        // Transitions
        stateMachine.TransitionToBasicAttack(stateMachine.InputHandler.BasicAttackInput);
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
