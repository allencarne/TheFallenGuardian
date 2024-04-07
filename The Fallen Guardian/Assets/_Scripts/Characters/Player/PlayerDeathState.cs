using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        Debug.Log("Death State");

        stateMachine.BodyAnimator.Play("Death");
        stateMachine.StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(.8f);

        // Reset Player Position
        stateMachine.transform.position = Vector2.zero;

        // Reset Player Health
        stateMachine.Player.playerStats.health = stateMachine.Player.playerStats.maxHealth;

        stateMachine.SetState(new PlayerSpawnState(stateMachine));
    }
}