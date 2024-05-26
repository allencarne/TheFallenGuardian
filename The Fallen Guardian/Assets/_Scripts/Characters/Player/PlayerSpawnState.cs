using System.Collections;
using UnityEngine;


public class PlayerSpawnState : PlayerState
{
    bool canSpawn = true;

    public PlayerSpawnState(PlayerStateMachine playerStateMachine): base(playerStateMachine) { }

    public override void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;

            // State Logic
            stateMachine.HeadAnimator.Play(stateMachine.Equipment.HeadIndex + "_Spawn");
            stateMachine.ChestAnimator.Play(stateMachine.Equipment.ChestIndex + "_Spawn");
            stateMachine.LegsAnimator.Play(stateMachine.Equipment.LegsIndex + "_Spawn");
            stateMachine.BodyAnimator.Play("Spawn");
            stateMachine.SwordAnimator.Play("Spawn");

            stateMachine.Player.OnHealthChanged?.Invoke();

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
