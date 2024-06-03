using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Alacrity : MonoBehaviour, IAlacrityable
{
    [SerializeField] Debuff_Impede impede;

    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;
    [SerializeField] GameObject alacrityParticlePrefab;
    GameObject alacrityParticle;

    [Header("Icon")]
    [SerializeField] GameObject buffPrefab;
    TextMeshProUGUI stacksText;
    GameObject buffIcon;

    [Header("Alacrity")]
    public int activeAlacrityAmount = 0;
    int alacrityPerStack = 3;
    int alacrityStacks = 0;
    int alacrityAmount = 0;

    [Header("Character")]
    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Alacrity(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        alacrityStacks += stacks;

        // Cap the Stacks at 25
        alacrityStacks = Mathf.Min(alacrityStacks, 25);

        // Gain Based on Stack Amount
        ApplyAlacrity(alacrityStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buffPrefab, buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!alacrityParticle)
        {
            alacrityParticle = Instantiate(alacrityParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = alacrityStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        alacrityStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        alacrityStacks = Mathf.Max(alacrityStacks, 0);

        if (alacrityStacks == 0)
        {
            ResetAlacrity();
            Destroy(buffIcon);
            Destroy(alacrityParticle);
        }
        else
        {
            ApplyAlacrity(alacrityStacks);

            // Stacks Text
            stacksText.text = alacrityStacks.ToString();
        }
    }

    void ApplyAlacrity(int stacks)
    {
        // Calculate the amount based on the number of stacks
        alacrityAmount = alacrityPerStack * stacks;

        if (isPlayer)
        {
            activeAlacrityAmount = alacrityAmount;
        }
        else
        {
            activeAlacrityAmount = alacrityAmount;
        }

        Calculate();
    }

    void Calculate()
    {
        if (isPlayer)
        {
            playerStats.CurrentCDR = playerStats.BaseCDR + activeAlacrityAmount - impede.activeImpedeAmount;
        }
        else
        {
            enemy.CurrentCDR = enemy.BaseCDR + activeAlacrityAmount - impede.activeImpedeAmount;
        }
    }

    void ResetAlacrity()
    {
        if (isPlayer)
        {
            activeAlacrityAmount = 0;
        }
        else
        {
            activeAlacrityAmount = 0;
        }

        Calculate();
    }
}
