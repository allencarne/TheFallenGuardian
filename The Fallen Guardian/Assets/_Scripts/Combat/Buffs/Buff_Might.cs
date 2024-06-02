using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Might : MonoBehaviour, IMightable
{
    [SerializeField] Debuff_Weakness weakness;

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
    public int activeMightAmount = 0;

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
            activeMightAmount = mightAmount;
        }
        else
        {
            activeMightAmount = mightAmount;
        }

        // Recalculate damage
        RecalculateDamage();
    }

    void RecalculateDamage()
    {
        if (isPlayer)
        {
            playerStats.CurrentDamage = playerStats.BaseDamage + activeMightAmount - weakness.activeWeaknessAmount;
        }
        else
        {
            enemy.CurrentDamage = enemy.BaseDamage + activeMightAmount - weakness.activeWeaknessAmount;
        }
    }

    void ResetMight()
    {
        // Reset active might amount to zero in PlayerStats or Enemy
        if (isPlayer)
        {
            activeMightAmount = 0;
        }
        else
        {
            activeMightAmount = 0;
        }

        // Recalculate damage
        RecalculateDamage();
    }
}
