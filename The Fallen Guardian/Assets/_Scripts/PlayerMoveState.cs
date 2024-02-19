using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Update()
    {
        // State Logic
        Debug.Log("Move State");

        stateMachine.BodyAnimator.Play("Move");

        // Set idle Animation after move
        if (stateMachine.InputHandler.MoveInput != Vector2.zero)
        {
            stateMachine.BodyAnimator.SetFloat("Horizontal", stateMachine.InputHandler.MoveInput.x);
            stateMachine.BodyAnimator.SetFloat("Vertical", stateMachine.InputHandler.MoveInput.y);
        }

        // Transitions
    }

    public override void FixedUpdate()
    {
        HandleMovement(stateMachine.InputHandler.MoveInput);

        // Tansition to Idle State
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
