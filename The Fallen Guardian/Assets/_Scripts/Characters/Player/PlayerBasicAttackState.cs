using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicAttackState : PlayerState
{
    IAbilityBehaviour behaviour;

    public PlayerBasicAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        behaviour = stateMachine.Abilities.basicAbility;
    }

    public override void Update()
    {
        if (behaviour != null)
        {
            behaviour.BehaviourUpdate(stateMachine);
        }
    }
}
