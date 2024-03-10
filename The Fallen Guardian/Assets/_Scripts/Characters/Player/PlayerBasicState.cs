using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicState : PlayerState
{
    IAbilityBehaviour behaviour;

    public PlayerBasicState(PlayerStateMachine stateMachine) : base(stateMachine)
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
