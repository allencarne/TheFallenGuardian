using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debuff_Weakness : MonoBehaviour, IWeaknessable
{
    [SerializeField] Buff_Might might;

    [Header("DeBuff Bar")]
    [SerializeField] GameObject deBuffBar;
    [SerializeField] GameObject weaknessParticlePrefab;
    GameObject weaknessParticle;

    [Header("Icon")]
    [SerializeField] GameObject debuffPrefab;
    TextMeshProUGUI stacksText;
    GameObject buffIcon;

    [Header("Weakness")]
    public int activeWeaknessAmount = 0;
    int damagePerStack = 3;
    int weaknessStacks = 0;
    int weaknessAmount = 0;

    [Header("Character")]
    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Weakness(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        weaknessStacks += stacks;

        // Cap the Stacks at 25
        weaknessStacks = Mathf.Min(weaknessStacks, 25);

        // Gain Based on Stack Amount
        ApplyWeakness(weaknessStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(debuffPrefab, deBuffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!weaknessParticle)
        {
            weaknessParticle = Instantiate(weaknessParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = weaknessStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        weaknessStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        weaknessStacks = Mathf.Max(weaknessStacks, 0);

        if (weaknessStacks == 0)
        {
            ResetWeakness();
            Destroy(buffIcon);
            Destroy(weaknessParticle);
        }
        else
        {
            ApplyWeakness(weaknessStacks);

            // Stacks Text
            stacksText.text = weaknessStacks.ToString();
        }
    }

    void ApplyWeakness(int stacks)
    {
        // Calculate the new weakness amount based on the number of stacks
        weaknessAmount = damagePerStack * stacks;

        // Update the active weakness amount in the PlayerStats or Enemy
        if (isPlayer)
        {
            activeWeaknessAmount = weaknessAmount;
        }
        else
        {
            activeWeaknessAmount = weaknessAmount;
        }

        // Recalculate damage
        RecalculateDamage();
    }

    void RecalculateDamage()
    {
        if (isPlayer)
        {
            playerStats.CurrentDamage = playerStats.BaseDamage + might.activeMightAmount - activeWeaknessAmount;
        }
        else
        {
            enemy.CurrentDamage = enemy.BaseDamage + might.activeMightAmount - activeWeaknessAmount;
        }
    }

    void ResetWeakness()
    {
        // Reset active weakness amount to zero in PlayerStats or Enemy
        if (isPlayer)
        {
            activeWeaknessAmount = 0;
        }
        else
        {
            activeWeaknessAmount = 0;
        }

        // Recalculate damage
        RecalculateDamage();
    }
}
