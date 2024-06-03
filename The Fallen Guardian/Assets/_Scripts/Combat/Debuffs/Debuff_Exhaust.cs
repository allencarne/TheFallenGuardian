using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debuff_Exhaust : MonoBehaviour, IExhaustable
{
    [SerializeField] Buff_Swiftness swiftness;

    [Header("DeBuff Bar")]
    [SerializeField] GameObject deBuffBar;
    [SerializeField] GameObject exhaustParticlePrefab;
    GameObject exhaustParticle;

    [Header("Icon")]
    [SerializeField] GameObject debuffPrefab;
    TextMeshProUGUI stacksText;
    GameObject buffIcon;

    [Header("Exhaust")]
    public float activeExhaustAmount = 0;
    float exhaustPerStack = .1f;
    int exhaustStacks = 0;
    float exhaustAmount = 0;

    [Header("Character")]
    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Exhaust(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        exhaustStacks += stacks;

        // Cap the Stacks at 25
        exhaustStacks = Mathf.Min(exhaustStacks, 25);

        // Gain Based on Stack Amount
        ApplyExhaust(exhaustStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(debuffPrefab, deBuffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!exhaustParticle)
        {
            exhaustParticle = Instantiate(exhaustParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = exhaustStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        exhaustStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        exhaustStacks = Mathf.Max(exhaustStacks, 0);

        if (exhaustStacks == 0)
        {
            ResetWeakness();
            Destroy(buffIcon);
            Destroy(exhaustParticle);
        }
        else
        {
            ApplyExhaust(exhaustStacks);

            // Stacks Text
            stacksText.text = exhaustStacks.ToString();
        }
    }

    void ApplyExhaust(int stacks)
    {
        // Calculate the new amount based on the number of stacks
        exhaustAmount = exhaustPerStack * stacks;

        // Update the active amount in the PlayerStats or Enemy
        if (isPlayer)
        {
            activeExhaustAmount = exhaustAmount;
        }
        else
        {
            activeExhaustAmount = exhaustAmount;
        }

        // Recalculate damage
        RecalculateExhaust();
    }

    void RecalculateExhaust()
    {
        if (isPlayer)
        {
            playerStats.CurrentAttackSpeed = Mathf.Max(.3f,playerStats.BaseAttackSpeed + swiftness.activeSwitfnessAmount - activeExhaustAmount);
        }
        else
        {
            enemy.CurrentAttackSpeed = Mathf.Max(.3f,enemy.BaseAttackSpeed + swiftness.activeSwitfnessAmount - activeExhaustAmount);
        }
    }

    void ResetWeakness()
    {
        // Reset active amount to zero in PlayerStats or Enemy
        if (isPlayer)
        {
            activeExhaustAmount = 0;
        }
        else
        {
            activeExhaustAmount = 0;
        }

        // Recalculate damage
        RecalculateExhaust();
    }
}
