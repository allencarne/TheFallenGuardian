using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    #region Basic Ability
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
    #endregion

    #region Offensive Ability
    public ScriptableObject offensiveAbilityReference
    {
        get => _offensiveAbilityReference;
        set
        {
            _offensiveAbilityReference = value;
            offensiveAbility = (IBasicAttackBehaviour)_offensiveAbilityReference;
        }
    }
    private ScriptableObject _offensiveAbilityReference;
    public IBasicAttackBehaviour offensiveAbility { get; private set; }
    #endregion

    private void Awake()
    {
        basicAttackBehaviour = (IBasicAttackBehaviour)basicAttackBehaviourReference;
        offensiveAbility = (IBasicAttackBehaviour)offensiveAbilityReference;
    }
}
