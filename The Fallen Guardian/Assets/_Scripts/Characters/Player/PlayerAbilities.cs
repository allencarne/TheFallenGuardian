using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public ScriptableObject basicAttackBehaviourReference
    {
        get => _basicAttackBehaviourReference;
        set
        {
            _basicAttackBehaviourReference = value;
            basicAttackBehaviour = (IBasicAttackBehaviour)_basicAttackBehaviourReference;
        }
    }
    private ScriptableObject _basicAttackBehaviourReference;
    public IBasicAttackBehaviour basicAttackBehaviour { get; private set; }

    private void Awake()
    {
        basicAttackBehaviour = (IBasicAttackBehaviour)basicAttackBehaviourReference;
    }
}
