using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Update()
    {
        stateMachine.HandleAnimation("Sword","Move");
        
        stateMachine.BodyAnimator.Play("Move");

        // Set idle Animation after move
        if (stateMachine.InputHandler.MoveInput != Vector2.zero)
        {
            stateMachine.BodyAnimator.SetFloat("Horizontal", stateMachine.InputHandler.MoveInput.x);
            stateMachine.BodyAnimator.SetFloat("Vertical", stateMachine.InputHandler.MoveInput.y);
        }
        /*
        stateMachine.ClubAnimator.Play("Move");

        // Set idle Animation after move
        if (stateMachine.InputHandler.MoveInput != Vector2.zero)
        {
            // Map horizontal and vertical input values to discrete values (-1, 0, 1)
            float horizontalValue = Mathf.Round(stateMachine.InputHandler.MoveInput.x);
            float verticalValue = Mathf.Round(stateMachine.InputHandler.MoveInput.y);

            stateMachine.ClubAnimator.SetFloat("Horizontal", horizontalValue);
            stateMachine.ClubAnimator.SetFloat("Vertical", verticalValue);
        }

        stateMachine.SwordAnimator.Play("Move");

        // Set idle Animation after move
        if (stateMachine.InputHandler.MoveInput != Vector2.zero)
        {
            // Map horizontal and vertical input values to discrete values (-1, 0, 1)
            float horizontalValue = Mathf.Round(stateMachine.InputHandler.MoveInput.x);
            float verticalValue = Mathf.Round(stateMachine.InputHandler.MoveInput.y);

            stateMachine.SwordAnimator.SetFloat("Horizontal", horizontalValue);
            stateMachine.SwordAnimator.SetFloat("Vertical", verticalValue);
        }
        */
        // Transitions
        stateMachine.TransitionToBasicAttack(stateMachine.InputHandler.BasicAttackInput);
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
