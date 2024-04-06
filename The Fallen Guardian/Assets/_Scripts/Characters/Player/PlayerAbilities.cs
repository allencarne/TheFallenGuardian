using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    #region Basic Ability
    public ScriptableObject basicAbilityReference
    {
        get => _basicAbilityReference;
        set
        {
            _basicAbilityReference = value;
            basicAbility = (IAbilityBehaviour)_basicAbilityReference;
        }
    }
    private ScriptableObject _basicAbilityReference;
    public IAbilityBehaviour basicAbility { get; private set; }
    #endregion

    #region Offensive Ability
    public ScriptableObject offensiveAbilityReference
    {
        get => _offensiveAbilityReference;
        set
        {
            _offensiveAbilityReference = value;
            offensiveAbility = (IAbilityBehaviour)_offensiveAbilityReference;
        }
    }
    private ScriptableObject _offensiveAbilityReference;
    public IAbilityBehaviour offensiveAbility { get; private set; }
    #endregion

    #region Mobility Ability
    public ScriptableObject mobilityAbilityReference
    {
        get => _mobilityAbilityReference;
        set
        {
            _mobilityAbilityReference = value;
            mobilityAbility = (IAbilityBehaviour)_mobilityAbilityReference;
        }
    }
    private ScriptableObject _mobilityAbilityReference;
    public IAbilityBehaviour mobilityAbility { get; private set; }
    #endregion

    private void Awake()
    {
        basicAbility = (IAbilityBehaviour)basicAbilityReference;
        offensiveAbility = (IAbilityBehaviour)offensiveAbilityReference;
        mobilityAbility = (IAbilityBehaviour)mobilityAbilityReference;
    }
}
