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

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    int CurrentDamage;
    int BaseDamage;

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
            buffIcon = Instantiate(buff_Might);
            buffIcon.transform.SetParent(buffBar.transform);
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
            MightStacks = 0;
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
        SetValues();

        // Calculate the amount based on the number of stacks
        int MightAmount = DamagePerStack * stacks;

        // Calculate the total damage increase considering the Weakness debuff
        int totalDamageIncrease = MightAmount - (weakness.WeaknessStacks * weakness.DamagePerStack);

        CurrentDamage = BaseDamage + totalDamageIncrease;

        if (isPlayer)
        {
            playerStats.CurrentDamage = CurrentDamage;
        }
        else
        {
            enemy.CurrentDamage = CurrentDamage;
        }
    }

    void ResetMight()
    {
        if (isPlayer)
        {
            // Calculate the remaining effect of the Weakness debuff
            int remainingWeaknessEffect = weakness.WeaknessStacks * weakness.DamagePerStack;

            // Revert the current damage back to the base level minus the remaining Weakness effect
            playerStats.CurrentDamage = playerStats.BaseDamage - remainingWeaknessEffect;
        }
        else
        {
            // Similar logic for enemies, but you might need to adjust based on your game's design
            //enemy.CurrentDamage = enemy.BaseDamage - remainingWeaknessEffect;
        }
    }

    void SetValues()
    {
        if (isPlayer)
        {
            CurrentDamage = playerStats.CurrentDamage;
            BaseDamage = playerStats.BaseDamage;
        }
        else
        {
            CurrentDamage = enemy.CurrentDamage;
            BaseDamage = enemy.BaseDamage;
        }
    }
}
