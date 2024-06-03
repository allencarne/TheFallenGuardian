using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debuff_Slow : MonoBehaviour, ISlowable
{
    [SerializeField] Buff_Haste haste;

    [Header("DeBuff Bar")]
    [SerializeField] GameObject deBuffBar;
    [SerializeField] GameObject slowParticlePrefab;
    GameObject slowParticle;

    [Header("Icon")]
    [SerializeField] GameObject debuffPrefab;
    TextMeshProUGUI stacksText;
    GameObject debuffIcon;

    [Header("Slow")]
    public int activeSlowAmount = 0;
    int slowPerStack = 1;
    int slowStacks = 0;
    int slowAmount = 0;

    [Header("Character")]
    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Slow(int stacks, float duration)
    {
        // Increase Amount Based on the New Buff
        slowStacks += stacks;

        // Cap the Stacks at 25
        slowStacks = Mathf.Min(slowStacks, 25);

        // Gain Based on Stack Amount
        ApplySlow(slowStacks);

        // Icon
        if (!debuffIcon)
        {
            debuffIcon = Instantiate(debuffPrefab, deBuffBar.transform);
            debuffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = debuffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!slowParticle)
        {
            slowParticle = Instantiate(slowParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = slowStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        slowStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        slowStacks = Mathf.Max(slowStacks, 0);

        if (slowStacks == 0)
        {
            ResetHaste();
            Destroy(debuffIcon);
            Destroy(slowParticle);
        }
        else
        {
            ApplySlow(slowStacks);

            // Stacks Text
            stacksText.text = slowStacks.ToString();
        }
    }

    void ApplySlow(int stacks)
    {
        slowAmount = slowPerStack * stacks;

        if (isPlayer)
        {
            activeSlowAmount = slowAmount;
        }
        else
        {
            activeSlowAmount = slowAmount;
        }

        RecalculateSpeed();
    }

    void RecalculateSpeed()
    {
        if (isPlayer)
        {
            playerStats.CurrentSpeed = Mathf.Max(1, playerStats.BaseSpeed + haste.activeHasteAmount - activeSlowAmount);
        }
        else
        {
            enemy.CurrentSpeed = Mathf.Max(1, enemy.BaseSpeed + haste.activeHasteAmount - activeSlowAmount);
        }
    }


    void ResetHaste()
    {
        if (isPlayer)
        {
            activeSlowAmount = 0;
        }
        else
        {
            activeSlowAmount = 0;
        }

        RecalculateSpeed();
    }
}