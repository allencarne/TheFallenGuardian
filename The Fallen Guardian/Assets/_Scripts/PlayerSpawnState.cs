using System.Collections;
using UnityEngine;


public class PlayerSpawnState : PlayerState
{
    bool canSpawn = true;

    public PlayerSpawnState(PlayerStateMachine playerStateMachine): base(playerStateMachine)
    {

    }

    public override void Update()
    {
        Debug.Log("Spawn State");

        if (canSpawn)
        {
            canSpawn = false;

            // State Logic
            stateMachine.BodyAnimator.Play("Spawn");
            stateMachine.StartCoroutine(SpawnDuration());
        }
    }

    IEnumerator SpawnDuration()
    {
        yield return new WaitForSeconds(.6f);

        canSpawn = true;

        stateMachine.SetState(new PlayerIdleState(stateMachine));
    }
}
