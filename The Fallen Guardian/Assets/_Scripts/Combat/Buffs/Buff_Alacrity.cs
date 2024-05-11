using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Alacrity : MonoBehaviour, IAlacrityable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;

    [Header("Icon")]
    [SerializeField] GameObject buff_Alacrity;
    TextMeshProUGUI stacksText;

    [Header("Haste")]
    float cdrPerStack = 3f;
    int alacrityStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    float CurrentCDR;
    float BaseCDR;

    private void Start()
    {
        if (isPlayer)
        {
            CurrentCDR = playerStats.CurrentCDR;
            BaseCDR = playerStats.BaseCDR;
        }
        else
        {
            CurrentCDR = enemy.CurrentCDR;
            BaseCDR = enemy.BaseCDR;
        }
    }

    public void Alacrity(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        alacrityStacks += stacks;

        // Cap the hasteStacks at 25
        alacrityStacks = Mathf.Min(alacrityStacks, 25);

        // Gain Haste Based on Stack Amount
        ApplyAlacrity(alacrityStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buff_Alacrity);
            buffIcon.transform.SetParent(buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        // Stacks Text
        stacksText.text = alacrityStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our HasteStacks
        alacrityStacks -= stacks;

        // Ensure hasteStacks doesn't go below zero
        alacrityStacks = Mathf.Max(alacrityStacks, 0);

        if (alacrityStacks == 0)
        {
            alacrityStacks = 0;
            ResetAlacrity();
            Destroy(buffIcon);
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
        // Calculate the haste amount based on the number of stacks
        float hasteAmount = cdrPerStack * stacks; // Increase movement speed by 1 for each stack

        CurrentCDR = BaseCDR + hasteAmount;

        if (isPlayer)
        {
            playerStats.CurrentCDR = CurrentCDR;
        }
        else
        {
            enemy.CurrentCDR = CurrentCDR;
        }
    }

    void ResetAlacrity()
    {
        if (isPlayer)
        {
            playerStats.CurrentCDR = playerStats.BaseCDR;
        }
        else
        {
            enemy.CurrentCDR = enemy.BaseCDR;
        }
    }
}
