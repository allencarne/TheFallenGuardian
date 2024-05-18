using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Swiftness : MonoBehaviour, ISwiftnessable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;
    [SerializeField] GameObject swiftnessParticlePrefab;
    GameObject swiftnessParticle;

    [Header("Icon")]
    [SerializeField] GameObject buff_Swiftness;
    TextMeshProUGUI stacksText;

    [Header("Swiftness")]
    float attackSpeedPerStack = .5f;
    int swiftnessStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    float CurrentAttackSpeed;
    float BaseAttackSpeed;

    private void Start()
    {
        if (isPlayer)
        {
            CurrentAttackSpeed = playerStats.CurrentAttackSpeed;
            BaseAttackSpeed = playerStats.BaseAttackSpeed;
        }
        else
        {
            CurrentAttackSpeed = enemy.CurrentAttackSpeed;
            BaseAttackSpeed = enemy.BaseAttackSpeed;
        }
    }

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
            buffIcon = Instantiate(buff_Swiftness);
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
            swiftnessStacks = 0;
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
        // Calculate the haste amount based on the number of stacks
        float swiftnessAmount = attackSpeedPerStack * stacks; // Increase movement speed by 1 for each stack

        CurrentAttackSpeed = BaseAttackSpeed + swiftnessAmount;

        if (isPlayer)
        {
            playerStats.CurrentAttackSpeed = CurrentAttackSpeed;
        }
        else
        {
            enemy.CurrentAttackSpeed = CurrentAttackSpeed;
        }
    }

    void ResetSwiftness()
    {
        if (isPlayer)
        {
            playerStats.CurrentAttackSpeed = playerStats.BaseAttackSpeed;
        }
        else
        {
            enemy.CurrentAttackSpeed = enemy.BaseAttackSpeed;
        }
    }
}
