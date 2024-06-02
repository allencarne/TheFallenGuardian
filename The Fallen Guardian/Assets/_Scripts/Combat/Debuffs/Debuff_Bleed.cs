using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debuff_Bleed : MonoBehaviour, IBleedable
{
    [Header("DeBuff Bar")]
    [SerializeField] GameObject bleedParticlePrefab;
    GameObject bleedParticle;
    [SerializeField] GameObject deBuffBar;

    [Header("Icon")]
    [SerializeField] GameObject deBuff_Bleed;
    TextMeshProUGUI stacksText;

    [Header("Bleed")]
    float healthPerStack = 1f;
    int bleedStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Player player;
    [SerializeField] Enemy enemy;

    public void Bleed(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        bleedStacks += stacks;

        // Cap the Stacks at 25
        bleedStacks = Mathf.Min(bleedStacks, 25);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(deBuff_Bleed);
            buffIcon.transform.SetParent(deBuffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!bleedParticle)
        {
            bleedParticle = Instantiate(bleedParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = bleedStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        int seconds = Mathf.CeilToInt(duration);
        for (int i = 0; i < seconds; i++)
        {
            ApplyBleed(stacks);
            yield return new WaitForSeconds(1f);
        }

        // Subtract the Stack from our Stacks
        bleedStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        bleedStacks = Mathf.Max(bleedStacks, 0);

        if (bleedStacks == 0)
        {
            Destroy(buffIcon);
            Destroy(bleedParticle);
        }
        else
        {
            // Update stacks text
            stacksText.text = bleedStacks.ToString();
        }
    }

    void ApplyBleed(int stacks)
    {
        // Calculate the amount based on the number of stacks
        float bleedAmount = healthPerStack * stacks;

        if (isPlayer)
        {
            // Apply Repeatedly once per second
            player.TakeDamage(bleedAmount);
        }
        else
        {
            // Apply to enemy if necessary
            enemy.TakeDamage(bleedAmount);
        }
    }
}
