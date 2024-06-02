using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Might : MonoBehaviour, IMightable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;
    [SerializeField] GameObject mightParticlePrefab;
    GameObject mightParticle;

    [Header("Icon")]
    [SerializeField] GameObject buff_Might;
    TextMeshProUGUI stacksText;

    [Header("Might")]
    public int DamagePerStack = 3;
    public int MightStacks = 0;
    GameObject buffIcon;
    int mightAmount = 0;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;

    public void Might(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        MightStacks += stacks;

        // Cap the Stacks at 25
        MightStacks = Mathf.Min(MightStacks, 25);

        // Gain Based on Stack Amount
        ApplyMight(MightStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buff_Might, buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!mightParticle)
        {
            mightParticle = Instantiate(mightParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = MightStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our Stacks
        MightStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        MightStacks = Mathf.Max(MightStacks, 0);

        if (MightStacks == 0)
        {
            ResetMight();
            Destroy(buffIcon);
            Destroy(mightParticle);
        }
        else
        {
            ApplyMight(MightStacks);

            // Stacks Text
            stacksText.text = MightStacks.ToString();
        }
    }

    void ApplyMight(int stacks)
    {
        // Calculate the new might amount based on the number of stacks
        mightAmount = DamagePerStack * stacks;

        // Update the active might amount in the PlayerStats or Enemy
        if (isPlayer)
        {
            playerStats.activeMightAmount = mightAmount;
        }
        else
        {
            enemy.activeMightAmount = mightAmount;
        }

        // Recalculate damage
        RecalculateDamage();
    }

    void RecalculateDamage()
    {
        if (isPlayer)
        {
            playerStats.CurrentDamage = playerStats.BaseDamage + playerStats.activeMightAmount - playerStats.activeWeaknessAmount;
        }
        else
        {
            enemy.CurrentDamage = enemy.BaseDamage + enemy.activeMightAmount - enemy.activeWeaknessAmount;
        }
    }

    void ResetMight()
    {
        // Reset active might amount to zero in PlayerStats or Enemy
        if (isPlayer)
        {
            playerStats.activeMightAmount = 0;
        }
        else
        {
            enemy.activeMightAmount = 0;
        }

        // Recalculate damage
        RecalculateDamage();
    }
}
