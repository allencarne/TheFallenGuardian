using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buff_Haste : MonoBehaviour, IHasteable
{
    [SerializeField] Debuff_Slow slow;

    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;
    [SerializeField] GameObject speedParticlePrefab;
    GameObject speedParticle;

    [Header("Icon")]
    [SerializeField] GameObject BuffPrefab;
    TextMeshProUGUI stacksText;
    GameObject buffIcon;

    [Header("Haste")]
    public int activeHasteAmount = 0;
    int hastePerStack = 1;
    int hasteStacks = 0;
    int hasteAmount = 0;

    [Header("Character")]
    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Haste(int stacks, float duration)
    {
        // Increase Amount Based on the New Buff
        hasteStacks += stacks;

        // Cap the Stacks at 25
        hasteStacks = Mathf.Min(hasteStacks, 25);

        // Gain Based on Stack Amount
        ApplyHaste(hasteStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(BuffPrefab, buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!speedParticle)
        {
            speedParticle = Instantiate(speedParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = hasteStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        hasteStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        hasteStacks = Mathf.Max(hasteStacks, 0);

        if (hasteStacks == 0)
        {
            ResetHaste();
            Destroy(buffIcon);
            Destroy(speedParticle);
        }
        else
        {
            ApplyHaste(hasteStacks);

            // Stacks Text
            stacksText.text = hasteStacks.ToString();
        }
    }

    void ApplyHaste(int stacks)
    {
        hasteAmount = hastePerStack * stacks;

        if (isPlayer)
        {
            activeHasteAmount = hasteAmount;
        }
        else
        {
            activeHasteAmount = hasteAmount;
        }

        RecalculateSpeed();
    }

    void RecalculateSpeed()
    {
        if (isPlayer)
        {
            playerStats.CurrentSpeed = playerStats.BaseSpeed + activeHasteAmount - slow.activeSlowAmount;
        }
        else
        {
            enemy.CurrentSpeed = enemy.BaseSpeed + activeHasteAmount - slow.activeSlowAmount;
        }
    }

    void ResetHaste()
    {
        if (isPlayer)
        {
            activeHasteAmount = 0;
        }
        else
        {
            activeHasteAmount = 0;
        }

        RecalculateSpeed();
    }
}