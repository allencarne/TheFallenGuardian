using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BeginnerAbilityTree : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerAbilities playerAbilities;

    [Header("Level 1")]
    [SerializeField] ScriptableObject basicAbility;
    public Image basicAbilityImage;
    public UnityEvent onBasicAbilitySelected;

    [Header("Level 5")]
    [SerializeField] ScriptableObject offensiveAbility;
    [SerializeField] ScriptableObject offensiveAbility2;

    private void Start()
    {
        // Set the level1Ability Icon to level1AbilityImage
        SetAbilityIcon(basicAbility, basicAbilityImage);

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

    public void OnLevel1AbilitySelected()
    {
        if (playerAbilities.basicAttackBehaviourReference == null)
        {
            playerAbilities.basicAttackBehaviourReference = basicAbility;

            onBasicAbilitySelected.Invoke();
        }
    }
}
