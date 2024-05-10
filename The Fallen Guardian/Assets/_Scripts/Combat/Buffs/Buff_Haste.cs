using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buff_Haste : MonoBehaviour, IHasteable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;

    [Header("Icon")]
    [SerializeField] GameObject buff_Haste;
    TextMeshProUGUI stacksText;

    [Header("Haste")]
    float speedPerStack = 3f;
    int hasteStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    float CurrentSpeed;
    float BaseSpeed;

    private void Start()
    {
        if (isPlayer)
        {
            CurrentSpeed = playerStats.CurrentSpeed;
            BaseSpeed = playerStats.BaseSpeed;
        }
        else
        {
            CurrentSpeed = enemy.CurrentSpeed;
            BaseSpeed = enemy.BaseSpeed;
        }
    }

    public void Haste(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        hasteStacks += stacks;

        // Cap the hasteStacks at 25
        hasteStacks = Mathf.Min(hasteStacks, 25);

        // Gain Haste Based on Stack Amount
        ApplyHaste(hasteStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buff_Haste);
            buffIcon.transform.SetParent(buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        // Stacks Text
        stacksText.text = hasteStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        // Subtrack the Stack from our HasteStacks
        hasteStacks -= stacks;

        // Ensure hasteStacks doesn't go below zero
        hasteStacks = Mathf.Max(hasteStacks, 0);

        if (hasteStacks == 0)
        {
            EndHaste();
            Destroy(buffIcon);
        }
        else
        {
            // Stacks Text
            stacksText.text = stacks.ToString();
        }
    }

    void ApplyHaste(int stacks)
    {
        // Calculate the haste amount based on the number of stacks
        float hasteAmount = speedPerStack * stacks; // Increase movement speed by 1 for each stack

        CurrentSpeed = BaseSpeed + hasteAmount;

        if (isPlayer)
        {
            playerStats.CurrentSpeed = CurrentSpeed;
        }
        else
        {
            enemy.CurrentSpeed = CurrentSpeed;
        }
    }

    public void EndHaste()
    {
        hasteStacks = 0;

        // Reset the speed
        ResetHaste();
    }

    void ResetHaste()
    {
        if (isPlayer)
        {
            playerStats.CurrentSpeed = playerStats.BaseSpeed;
        }
        else
        {
            enemy.CurrentSpeed = enemy.BaseSpeed;
        }
    }
}