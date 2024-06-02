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
    int CurrentDamage;

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
        weaknessAmount = DamagePerStack * stacks;

        CurrentDamage -= weaknessAmount;

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
            playerStats.CurrentDamage += weaknessAmount;
        }
        else
        {
            enemy.CurrentDamage += weaknessAmount;
        }
    }

    void SetValues()
    {
        if (isPlayer)
        {
            CurrentDamage = playerStats.CurrentDamage;
        }
        else
        {
            CurrentDamage = enemy.CurrentDamage;
        }
    }
}
