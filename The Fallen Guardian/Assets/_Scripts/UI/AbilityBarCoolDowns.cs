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
            if (playerAbilities.basicAbilityReference != null)
            {
                basicAbility = playerAbilities.basicAbilityReference;
                float cooldown = (float)basicAbility.GetType().GetField("coolDown").GetValue(basicAbility);
            }
        }
    }
}
