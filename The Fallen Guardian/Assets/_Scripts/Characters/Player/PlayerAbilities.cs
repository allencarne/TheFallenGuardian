using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public ScriptableObject basicAttackBehaviourReference;
    public IBasicAttackBehaviour basicAttackBehaviour { get; private set; }

    private void Awake()
    {
        basicAttackBehaviour = (IBasicAttackBehaviour)basicAttackBehaviourReference;
    }
}
