using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMobilityState : PlayerState
{
    IAbilityBehaviour behaviour;

    public PlayerMobilityState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        behaviour = stateMachine.Abilities.mobilityAbility;
    }

    public override void Update()
    {
        if (behaviour != null)
        {
            behaviour.BehaviourUpdate(stateMachine);
        }
    }
}
