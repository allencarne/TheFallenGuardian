using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttackState : PlayerState
{
    IBasicAttackBehaviour behaviour;

    public PlayerBasicAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        behaviour = stateMachine.Abilities.basicAttackBehaviour;
    }

    public override void Update()
    {
        behaviour.BehaviourUpdate(stateMachine);
    }
}
