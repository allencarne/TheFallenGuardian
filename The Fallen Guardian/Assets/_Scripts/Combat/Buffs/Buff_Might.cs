using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Might : MonoBehaviour, IMightable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;

    [Header("Icon")]
    [SerializeField] GameObject buff_Might;
    TextMeshProUGUI stacksText;

    [Header("Might")]
    int damagePerStack = 3;
    int mightStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    int CurrentDamage;
    int BaseDamage;

    private void Start()
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

    public void Might(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        mightStacks += stacks;

        // Cap the hasteStacks at 25
        mightStacks = Mathf.Min(mightStacks, 25);

        // Gain Haste Based on Stack Amount
        ApplyMight(mightStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buff_Might);
            buffIcon.transform.SetParent(buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        // Stacks Text
        stacksText.text = mightStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our HasteStacks
        mightStacks -= stacks;

        // Ensure hasteStacks doesn't go below zero
        mightStacks = Mathf.Max(mightStacks, 0);

        if (mightStacks == 0)
        {
            mightStacks = 0;
            ResetMight();
            Destroy(buffIcon);
        }
        else
        {
            ApplyMight(mightStacks);

            // Stacks Text
            stacksText.text = stacks.ToString();
        }
    }

    void ApplyMight(int stacks)
    {
        // Calculate the haste amount based on the number of stacks
        int MightAmount = damagePerStack * stacks; // Increase movement speed by 1 for each stack

        CurrentDamage = BaseDamage + MightAmount;

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
            playerStats.CurrentDamage = playerStats.BaseDamage;
        }
        else
        {
            enemy.CurrentDamage = enemy.BaseDamage;
        }
    }
}
