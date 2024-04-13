using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityBarCoolDowns : MonoBehaviour
{
    [SerializeField] AbilityBar abilityBar;

    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerAbilities playerAbilities;

    [Header("Basic")]
    [SerializeField] ScriptableObject basicAbility;
    [SerializeField] Image basicFill;
    [SerializeField] TextMeshProUGUI basicText;

    [Header("Offensive")]
    [SerializeField] Image offensiveFill;
    [SerializeField] TextMeshProUGUI offensiveText;

    [Header("Mobility")]
    [SerializeField] Image mobilityFill;
    [SerializeField] TextMeshProUGUI mobilityText;

    [Header("Defensive")]
    [SerializeField] Image defensiveFill;
    [SerializeField] TextMeshProUGUI defensiveText;

    [Header("Utility")]
    [SerializeField] Image utilityFill;
    [SerializeField] TextMeshProUGUI utilityText;

    [Header("Ultimate")]
    [SerializeField] Image ultimateFill;
    [SerializeField] TextMeshProUGUI ultimateText;

    public void OnPlayerJoin()
    {
        Invoke("GetAbilities", .5f);
    }

    void GetAbilities()
    {
        if (playerReference.items.Count > 0)
        {
            playerAbilities = playerReference.GetItemIndex(0).GetComponent<PlayerAbilities>();
        }
    }

    private void Update()
    {
        if (playerAbilities != null)
        {
            basicAbility = playerAbilities.basicAbilityReference;

            if (basicAbility != null)
            {
                UpdateCooldownUI(playerAbilities.basicAbilityReference, basicFill, basicText);
            }
        }
    }

    private void UpdateCooldownUI(ScriptableObject ability, Image fillImage, TextMeshProUGUI cooldownText)
    {
        if (ability != null)
        {
            float coolDown = (float)ability.GetType().GetField("coolDown").GetValue(ability);
            float coolDownTime = (float)ability.GetType().GetField("coolDownTime").GetValue(ability);

            if (coolDownTime > 0)
            {
                // Format cooldown time to show one decimal place if it's greater than 0
                cooldownText.text = coolDownTime.ToString("F1");
            }
            else
            {
                // Set text to empty if cooldown time is 0 or less
                cooldownText.text = "";
            }

            fillImage.fillAmount = coolDownTime / coolDown;
        }
    }
}
