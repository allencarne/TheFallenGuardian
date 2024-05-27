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
    int damagePerStack = 3;
    int weaknessStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    int CurrentDamage;
    int BaseDamage;

    private void Start()
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
            weaknessStacks = 0;
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
        // Calculate the amount based on the number of stacks
        int weaknessAmount = damagePerStack * stacks;

        CurrentDamage = BaseDamage - weaknessAmount;

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
            playerStats.CurrentDamage = playerStats.BaseDamage;
        }
        else
        {
            enemy.CurrentDamage = enemy.BaseDamage;
        }
    }
}
