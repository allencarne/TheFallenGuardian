using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Update()
    {
        stateMachine.HandleAnimation(stateMachine.BodyAnimator, "Player", "Move", stateMachine.InputHandler.MoveInput.normalized);
        stateMachine.HandleAnimation(stateMachine.SwordAnimator, "Sword", "Move", stateMachine.InputHandler.MoveInput.normalized);

        // Transitions
        stateMachine.TransitionToBasicAttack(stateMachine.InputHandler.BasicAttackInput);
    }

    public override void FixedUpdate()
    {
        HandleMovement(stateMachine.InputHandler.MoveInput);

        // If we are no longer moving - Transition to Idle State
        if (stateMachine.InputHandler.MoveInput == Vector2.zero)
        {
            stateMachine.SetState(new PlayerIdleState(stateMachine));
        }
    }

    void HandleMovement(Vector2 moveInput)
    {
        Vector2 movement = moveInput.normalized * stateMachine.Player.playerStats.movementSpeed;
        stateMachine.Rigidbody.velocity = movement;
    }
}
