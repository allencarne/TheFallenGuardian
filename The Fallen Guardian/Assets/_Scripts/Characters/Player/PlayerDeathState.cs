using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Update()
    {
        stateMachine.BodyAnimator.Play("Death");
    }
}