using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffensiveState : PlayerState
{
    IBasicAttackBehaviour behaviour;

    public PlayerOffensiveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        behaviour = stateMachine.Abilities.basicAttackBehaviour;
    }

    public override void Update()
    {
        if (behaviour != null)
        {
            behaviour.BehaviourUpdate(stateMachine);
        }
    }
}
