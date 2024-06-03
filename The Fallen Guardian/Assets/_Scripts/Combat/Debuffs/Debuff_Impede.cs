using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debuff_Impede : MonoBehaviour, IImpedable
{
    [SerializeField] Buff_Alacrity alacrity;

    [Header("DeBuff Bar")]
    [SerializeField] GameObject deBuffBar;
    [SerializeField] GameObject impedeParticlePrefab;
    GameObject impedeParticle;

    [Header("Icon")]
    [SerializeField] GameObject debuffPrefab;
    TextMeshProUGUI stacksText;
    GameObject buffIcon;

    [Header("Impede")]
    public int activeImpedeAmount = 0;
    int impedePerStack = 3;
    int impedeStacks = 0;
    int impedeAmount = 0;

    [Header("Character")]
    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Impede(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        impedeStacks += stacks;

        // Cap the Stacks at 25
        impedeStacks = Mathf.Min(impedeStacks, 25);

        // Gain Based on Stack Amount
        ApplyImpede(impedeStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(debuffPrefab, deBuffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!impedeParticle)
        {
            impedeParticle = Instantiate(impedeParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = impedeStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        impedeStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        impedeStacks = Mathf.Max(impedeStacks, 0);

        if (impedeStacks == 0)
        {
            ResetWeakness();
            Destroy(buffIcon);
            Destroy(impedeParticle);
        }
        else
        {
            ApplyImpede(impedeStacks);

            // Stacks Text
            stacksText.text = impedeStacks.ToString();
        }
    }

    void ApplyImpede(int stacks)
    {
        // Calculate the new weakness amount based on the number of stacks
        impedeAmount = impedePerStack * stacks;

        // Update the active weakness amount in the PlayerStats or Enemy
        if (isPlayer)
        {
            activeImpedeAmount = impedeAmount;
        }
        else
        {
            activeImpedeAmount = impedeAmount;
        }

        Calculate();
    }

    void Calculate()
    {
        if (isPlayer)
        {
            playerStats.CurrentCDR = playerStats.BaseCDR + alacrity.activeAlacrityAmount - activeImpedeAmount;
        }
        else
        {
            enemy.CurrentCDR = enemy.BaseCDR + alacrity.activeAlacrityAmount - activeImpedeAmount;
        }
    }

    void ResetWeakness()
    {
        // Reset active weakness amount to zero in PlayerStats or Enemy
        if (isPlayer)
        {
            activeImpedeAmount = 0;
        }
        else
        {
            activeImpedeAmount = 0;
        }

        Calculate();
    }
}
