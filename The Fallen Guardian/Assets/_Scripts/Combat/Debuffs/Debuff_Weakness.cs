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
    [SerializeField] GameObject deBuff_Weakness;
    TextMeshProUGUI stacksText;

    [Header("Weakness")]
    public int DamagePerStack = 3;
    public int WeaknessStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    int CurrentDamage;
    int BaseDamage;

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
            buffIcon = Instantiate(deBuff_Weakness);
            buffIcon.transform.SetParent(deBuffBar.transform);
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
            WeaknessStacks = 0;
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
        SetValues();

        // Calculate the amount based on the number of stacks
        int WeaknessAmount = DamagePerStack * stacks;

        // Directly apply the damage decrease without considering the Might buff
        CurrentDamage = CurrentDamage - WeaknessAmount;

        if (isPlayer)
        {
            playerStats.CurrentDamage = CurrentDamage;
        }
        else
        {
            enemy.CurrentDamage = CurrentDamage;
        }
    }

    void ResetWeakness()
    {
        if (isPlayer)
        {
            // Simply reset the current damage to the base damage
            playerStats.CurrentDamage = playerStats.BaseDamage;
        }
        else
        {
            // Similar logic for enemies
            enemy.CurrentDamage = enemy.BaseDamage;
        }
    }

    void SetValues()
    {
        if (isPlayer)
        {
            CurrentDamage = playerStats.CurrentDamage;
            BaseDamage = playerStats.BaseDamage;
        }
        else
        {
            CurrentDamage = enemy.CurrentDamage;
            BaseDamage = enemy.BaseDamage;
        }
    }
}
