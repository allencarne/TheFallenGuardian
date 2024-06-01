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
    float healthPerStack = 1f;
    int regenerationStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Player player;
    [SerializeField] Enemy enemy;

    public void Regenerate(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        regenerationStacks += stacks;

        // Cap the Stacks at 25
        regenerationStacks = Mathf.Min(regenerationStacks, 25);

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
        int seconds = Mathf.CeilToInt(duration);
        for (int i = 0; i < seconds; i++)
        {
            ApplyRegeneration(stacks);
            yield return new WaitForSeconds(1f);
        }

        // Subtract the Stack from our Stacks
        regenerationStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        regenerationStacks = Mathf.Max(regenerationStacks, 0);

        if (regenerationStacks == 0)
        {
            Destroy(buffIcon);
            Destroy(healthParticle);
        }
        else
        {
            // Update stacks text
            stacksText.text = regenerationStacks.ToString();
        }
    }

    void ApplyRegeneration(int stacks)
    {
        // Calculate the amount based on the number of stacks
        float regenAmount = healthPerStack * stacks;

        if (isPlayer)
        {
            // Apply Heal Repeatedly once per second
            player.Heal(regenAmount);
        }
        else
        {
            // Apply Heal to enemy if necessary
            enemy.Heal(regenAmount);
        }
    }
}
