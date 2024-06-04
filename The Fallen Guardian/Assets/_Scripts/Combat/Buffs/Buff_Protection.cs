using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Protection : MonoBehaviour, IProtectionable
{
    [SerializeField] Debuff_Vulnerability vulnerability;

    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;
    [SerializeField] GameObject protectionParticlePrefab;
    GameObject protectionParticle;

    [Header("Icon")]
    [SerializeField] GameObject buffPrefab;
    TextMeshProUGUI stacksText;
    GameObject buffIcon;

    [Header("Protection")]
    public int activeProtectionAmount = 0;
    int armorPerStack = 1;
    int protectionStacks = 0;
    int protectionAmount = 0;

    [Header("Character")]
    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Protection(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        protectionStacks += stacks;

        // Cap the Stacks at 25
        protectionStacks = Mathf.Min(protectionStacks, 25);

        // Gain Based on Stack Amount
        ApplyProtection(protectionStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buffPrefab, buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!protectionParticle)
        {
            protectionParticle = Instantiate(protectionParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = protectionStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        protectionStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        protectionStacks = Mathf.Max(protectionStacks, 0);

        if (protectionStacks == 0)
        {
            ResetProtection();
            Destroy(buffIcon);
            Destroy(protectionParticle);
        }
        else
        {
            ApplyProtection(protectionStacks);

            stacksText.text = protectionStacks.ToString();
        }
    }

    void ApplyProtection(int stacks)
    {
        protectionAmount = armorPerStack * stacks;

        if (isPlayer)
        {
            activeProtectionAmount = protectionAmount;
        }
        else
        {
            activeProtectionAmount = protectionAmount;
        }

        Calculate();
    }

    void Calculate()
    {
        if (isPlayer)
        {
            playerStats.CurrentArmor = playerStats.BaseArmor + activeProtectionAmount - vulnerability.activeVulnerabilityAmount;
        }
        else
        {
            enemy.CurrentArmor = enemy.BaseArmor + activeProtectionAmount - vulnerability.activeVulnerabilityAmount;
        }
    }

    void ResetProtection()
    {
        if (isPlayer)
        {
            activeProtectionAmount = 0;
        }
        else
        {
            activeProtectionAmount = 0;
        }

        Calculate();
    }
}
