using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Regeneration : MonoBehaviour, IRegenerationable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject healthParticlePrefab;
    GameObject healthParticle;
    [SerializeField] GameObject buffBar;

    [Header("Icon")]
    [SerializeField] GameObject buff_Regeneration;
    TextMeshProUGUI stacksText;

    [Header("Regeneration")]
    float healthPerStack = 3f;
    int regenerationStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    float CurrentRegen;
    float MaxRegen;

    public void Regenerate(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        regenerationStacks += stacks;

        // Cap the Stacks at 25
        regenerationStacks = Mathf.Min(regenerationStacks, 25);

        // Gain Based on Stack Amount
        ApplyRegeneration(regenerationStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buff_Regeneration);
            buffIcon.transform.SetParent(buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!healthParticle)
        {
            healthParticle = Instantiate(healthParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = regenerationStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        regenerationStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        regenerationStacks = Mathf.Max(regenerationStacks, 0);

        if (regenerationStacks == 0)
        {
            regenerationStacks = 0;
            ResetRegeneration();
            Destroy(buffIcon);
            Destroy(healthParticle);
        }
        else
        {
            ApplyRegeneration(regenerationStacks);

            // Stacks Text
            stacksText.text = regenerationStacks.ToString();
        }
    }

    void ApplyRegeneration(int stacks)
    {
        SetValues();

        // Calculate the amount based on the number of stacks
        float regenAmount = healthPerStack * stacks;

        CurrentRegen = MaxRegen + regenAmount;

        if (isPlayer)
        {
            playerStats.CurrentRegen = CurrentRegen;
        }
        else
        {
            enemy.CurrentRegen = CurrentRegen;
        }
    }

    void ResetRegeneration()
    {
        if (isPlayer)
        {
            playerStats.CurrentRegen = playerStats.BaseRegen;
        }
        else
        {
            enemy.CurrentRegen = enemy.BaseRegen;
        }
    }

    void SetValues()
    {
        if (isPlayer)
        {
            CurrentRegen = playerStats.CurrentRegen;
            MaxRegen = playerStats.BaseRegen;
        }
        else
        {
            CurrentRegen = enemy.CurrentRegen;
            MaxRegen = enemy.BaseRegen;
        }
    }
}
