using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BeginnerAbilityTree : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerReference;
    [SerializeField] PlayerStats stats;
    PlayerAbilities playerAbilities;

    [Header("Basic")]
    [SerializeField] ScriptableObject basicAbility;
    public Image basicAbilityImage;
    public UnityEvent onBasicAbilitySelected;

    [Header("Offensive")]
    [SerializeField] ScriptableObject offensiveAbility;
    public Image offensiveAbilityImage;
    public Button offensiveAbilityButton;
    public UnityEvent onOffensiveAbilitySelected;

    [Header("Offensive2")]
    [SerializeField] ScriptableObject offensiveAbility2;
    public Image offensiveAbility2Image;
    public Button offensiveAbility2Button;
    public UnityEvent onOffensiveAbility2Selected;

    [Header("Mobility")]
    [SerializeField] ScriptableObject mobilityAbility;
    public Image mobilityAbilityImage;
    public Button mobilityAbilityButton;
    public UnityEvent onMobilityAbilitySelected;

    private void Start()
    {
        // Set the level1Ability Icon to level1AbilityImage
        SetAbilityIcon(basicAbility, basicAbilityImage);
        SetAbilityIcon(offensiveAbility, offensiveAbilityImage);
        SetAbilityIcon(offensiveAbility2, offensiveAbility2Image);
        SetAbilityIcon(mobilityAbility, mobilityAbilityImage);

        InitializePlayerReference();
    }

    private void Update()
    {
        UpdateAbilityIcons();
    }

    public void UpdateAbilityIcons()
    {
        if (stats.PlayerLevel < 5)
        {
            offensiveAbilityButton.interactable = false;
            offensiveAbility2Button.interactable = false;
        }
        else
        {
            offensiveAbilityButton.interactable = true;
            offensiveAbility2Button.interactable = true;
        }

        if (stats.PlayerLevel < 10)
        {
            mobilityAbilityButton.interactable = false;
        }
        else
        {
            mobilityAbilityButton.interactable = true;
        }
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
        playerAbilities.offensiveAbilityReference = offensiveAbility;

        onOffensiveAbilitySelected.Invoke();
    }

    public void OnOffensiveAbility2Selected()
    {
        playerAbilities.offensiveAbilityReference = offensiveAbility2;

        onOffensiveAbility2Selected.Invoke();
    }

    public void OnMobilityAbilitySelected()
    {
        if (playerAbilities.mobilityAbilityReference == null)
        {
            playerAbilities.mobilityAbilityReference = mobilityAbility;

            onMobilityAbilitySelected.Invoke();
        }
    }
}
