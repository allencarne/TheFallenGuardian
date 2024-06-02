using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debuff_Weakness : MonoBehaviour, IWeaknessable
{
    [Header("DeBuff Bar")]
    [SerializeField] GameObject deBuffBar;
    [SerializeField] GameObject weaknessParticlePrefab;
    GameObject weaknessParticle;

    [Header("Icon")]
    [SerializeField] GameObject deBuff_Weakness;
    TextMeshProUGUI stacksText;

    [Header("Weakness")]
    public int DamagePerStack = 3;
    public int WeaknessStacks = 0;
    GameObject buffIcon;
    int weaknessAmount = 0;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Weakness(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        WeaknessStacks += stacks;

        // Cap the Stacks at 25
        WeaknessStacks = Mathf.Min(WeaknessStacks, 25);

        // Gain Based on Stack Amount
        ApplyWeakness(WeaknessStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(deBuff_Weakness, deBuffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!weaknessParticle)
        {
            weaknessParticle = Instantiate(weaknessParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = WeaknessStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        WeaknessStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        WeaknessStacks = Mathf.Max(WeaknessStacks, 0);

        if (WeaknessStacks == 0)
        {
            ResetWeakness();
            Destroy(buffIcon);
            Destroy(weaknessParticle);
        }
        else
        {
            ApplyWeakness(WeaknessStacks);

            // Stacks Text
            stacksText.text = WeaknessStacks.ToString();
        }
    }

    void ApplyWeakness(int stacks)
    {
        // Calculate the new weakness amount based on the number of stacks
        weaknessAmount = DamagePerStack * stacks;

        // Update the active weakness amount in the PlayerStats or Enemy
        if (isPlayer)
        {
            playerStats.activeWeaknessAmount = weaknessAmount;
        }
        else
        {
            enemy.activeWeaknessAmount = weaknessAmount;
        }

        // Recalculate damage
        RecalculateDamage();
    }

    void RecalculateDamage()
    {
        if (isPlayer)
        {
            playerStats.CurrentDamage = playerStats.BaseDamage + playerStats.activeMightAmount - playerStats.activeWeaknessAmount;
        }
        else
        {
            enemy.CurrentDamage = enemy.BaseDamage + enemy.activeMightAmount - enemy.activeWeaknessAmount;
        }
    }

    void ResetWeakness()
    {
        // Reset active weakness amount to zero in PlayerStats or Enemy
        if (isPlayer)
        {
            playerStats.activeWeaknessAmount = 0;
        }
        else
        {
            enemy.activeWeaknessAmount = 0;
        }

        // Recalculate damage
        RecalculateDamage();
    }
}
