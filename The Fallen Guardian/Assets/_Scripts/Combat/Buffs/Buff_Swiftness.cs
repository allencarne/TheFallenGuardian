using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Swiftness : MonoBehaviour, ISwiftnessable
{
    [SerializeField] Debuff_Exhaust exhaust;

    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;
    [SerializeField] GameObject swiftnessParticlePrefab;
    GameObject swiftnessParticle;

    [Header("Icon")]
    [SerializeField] GameObject buffPrefab;
    TextMeshProUGUI stacksText;
    GameObject buffIcon;

    [Header("Swiftness")]
    public float activeSwitfnessAmount = 0;
    float swiftnessPerStack = .1f;
    int swiftnessStacks = 0;
    float swiftnessAmount = 0;

    [Header("Character")]
    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Swiftness(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        swiftnessStacks += stacks;

        // Cap the hasteStacks at 25
        swiftnessStacks = Mathf.Min(swiftnessStacks, 25);

        // Gain Haste Based on Stack Amount
        ApplySwiftness(swiftnessStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buffPrefab);
            buffIcon.transform.SetParent(buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!swiftnessParticle)
        {
            swiftnessParticle = Instantiate(swiftnessParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = swiftnessStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our HasteStacks
        swiftnessStacks -= stacks;

        // Ensure hasteStacks doesn't go below zero
        swiftnessStacks = Mathf.Max(swiftnessStacks, 0);

        if (swiftnessStacks == 0)
        {
            ResetSwiftness();
            Destroy(buffIcon);
            Destroy(swiftnessParticle);
        }
        else
        {
            ApplySwiftness(swiftnessStacks);

            // Stacks Text
            stacksText.text = swiftnessStacks.ToString();
        }
    }

    void ApplySwiftness(int stacks)
    {
        swiftnessAmount = swiftnessPerStack * stacks;

        if (isPlayer)
        {
            activeSwitfnessAmount = swiftnessAmount;
        }
        else
        {
            activeSwitfnessAmount = swiftnessAmount;
        }

        RecalculateSwiftness();
    }

    void RecalculateSwiftness()
    {
        if (isPlayer)
        {
            playerStats.CurrentAttackSpeed = playerStats.BaseAttackSpeed + activeSwitfnessAmount - exhaust.activeExhaustAmount;
        }
        else
        {
            enemy.CurrentAttackSpeed = enemy.BaseAttackSpeed + activeSwitfnessAmount - exhaust.activeExhaustAmount;
        }
    }

    void ResetSwiftness()
    {
        if (isPlayer)
        {
            activeSwitfnessAmount = 0;
        }
        else
        {
            activeSwitfnessAmount = 0;
        }

        RecalculateSwiftness();
    }
}
