using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BeginnerAbilityTree : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerAbilities playerAbilities;

    [Header("Basic")]
    [SerializeField] ScriptableObject basicAbility;
    public Image basicAbilityImage;
    public UnityEvent onBasicAbilitySelected;

    [Header("Offensive")]
    [SerializeField] ScriptableObject offensiveAbility;
    public Image offensiveAbilityImage;
    public UnityEvent onOffensiveAbilitySelected;

    [Header("Offensive2")]
    [SerializeField] ScriptableObject offensiveAbility2;

    private void Start()
    {
        // Set the level1Ability Icon to level1AbilityImage
        SetAbilityIcon(basicAbility, basicAbilityImage);
        SetAbilityIcon(offensiveAbility, offensiveAbilityImage);

        InitializePlayerReference();
    }

    private void SetAbilityIcon(ScriptableObject ability, Image abilityImage)
    {
        if (ability != null && abilityImage != null)
        {
            // Assuming the ability has a field named 'icon' of type Sprite
            Sprite icon = (Sprite)ability.GetType().GetField("icon").GetValue(ability);
            if (icon != null)
            {
                abilityImage.sprite = icon;
            }
            else
            {
                Debug.LogWarning("Icon not found for the ability: " + ability.name);
            }
        }
    }

    public void InitializePlayerReference()
    {
        if (playerReference.items.Count > 0)
        {
            playerAbilities = playerReference.GetItemIndex(0).GetComponent<PlayerAbilities>();
        }
    }

    public void OnBasicAbilitySelected()
    {
        if (playerAbilities.basicAbilityReference == null)
        {
            playerAbilities.basicAbilityReference = basicAbility;

            onBasicAbilitySelected.Invoke();
        }
    }

    public void OnOffensiveAbilitySelected()
    {
        if (playerAbilities.offensiveAbilityReference == null)
        {
            playerAbilities.offensiveAbilityReference = offensiveAbility;

            onOffensiveAbilitySelected.Invoke();
        }
    }
}
