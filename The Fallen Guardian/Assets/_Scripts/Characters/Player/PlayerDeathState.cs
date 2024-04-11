using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    bool hasExecuted = false;

    public override void Update()
    {
        if (!hasExecuted)
        {
            hasExecuted = true;

            stateMachine.BodyAnimator.Play("Death");
            stateMachine.StartCoroutine(DeathDelay());
        }
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(.8f);

        hasExecuted = false;

        // Reset Player Position
        stateMachine.transform.position = Vector2.zero;

        // Reset Player Health
        stateMachine.Player.Stats.Health = stateMachine.Player.Stats.MaxHealth;

        stateMachine.SetState(new PlayerSpawnState(stateMachine));
    }
}