using System.Collections;
using UnityEngine;


public class PlayerSpawnState : PlayerState
{
    public PlayerSpawnState(PlayerStateMachine playerStateMachine): base(playerStateMachine)
    {

    }

    public override void Update()
    {
        Debug.Log("Spawn State");

        // State Logic
        stateMachine.BodyAnimator.Play("Spawn");
        stateMachine.StartCoroutine(SpawnDuration());
    }

    IEnumerator SpawnDuration()
    {
        yield return new WaitForSeconds(.6f);

        stateMachine.SetState(new PlayerIdleState(stateMachine));
    }
}
