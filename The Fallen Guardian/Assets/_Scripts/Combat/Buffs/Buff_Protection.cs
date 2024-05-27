using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Protection : MonoBehaviour, IProtectionable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject armorParticlePrefab;
    GameObject armorParticle;
    [SerializeField] GameObject buffBar;

    [Header("Icon")]
    [SerializeField] GameObject buff_Protection;
    TextMeshProUGUI stacksText;

    [Header("Haste")]
    float armorPerStack = 3f;
    int protectionStacks = 0;
    GameObject buffIcon;

    public bool isPlayer;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Enemy enemy;
    float CurrentArmor;
    float BaseArmor;

    private void Start()
    {
        if (isPlayer)
        {
            CurrentArmor = playerStats.CurrentArmor;
            BaseArmor = playerStats.BaseArmor;
        }
        else
        {
            CurrentArmor = enemy.CurrentArmor;
            BaseArmor = enemy.BaseArmor;
        }
    }

    public void Protection(int stacks, float duration)
    {
        // Increase Stack Amount Based on the New Buff
        protectionStacks += stacks;

        // Cap the Stacks at 25
        protectionStacks = Mathf.Min(protectionStacks, 25);

        // Gain Based on Stack Amount
        ApplyHaste(protectionStacks);

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buff_Protection);
            buffIcon.transform.SetParent(buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);

            // Get Stacks Text
            stacksText = buffIcon.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!armorParticle)
        {
            armorParticle = Instantiate(armorParticlePrefab, transform);
        }

        // Stacks Text
        stacksText.text = protectionStacks.ToString();

        // Start a Timer for Each Instance of the Buff
        StartCoroutine(Stack(stacks, duration));
    }

    IEnumerator Stack(int stacks, float duration)
    {
        yield return new WaitForSeconds(duration);

        protectionStacks -= stacks;

        // Ensure Stacks doesn't go below zero
        protectionStacks = Mathf.Max(protectionStacks, 0);

        if (protectionStacks == 0)
        {
            protectionStacks = 0;
            ResetHaste();
            Destroy(buffIcon);
            Destroy(armorParticle);
        }
        else
        {
            ApplyHaste(protectionStacks);

            stacksText.text = protectionStacks.ToString();
        }
    }

    void ApplyHaste(int stacks)
    {
        float protectionAmount = armorPerStack * stacks;

        CurrentArmor = BaseArmor + protectionAmount;

        if (isPlayer)
        {
            playerStats.CurrentArmor = CurrentArmor;
        }
        else
        {
            enemy.CurrentArmor = CurrentArmor;
        }
    }

    void ResetHaste()
    {
        if (isPlayer)
        {
            playerStats.CurrentArmor = playerStats.BaseArmor;
        }
        else
        {
            enemy.CurrentArmor = enemy.BaseArmor;
        }
    }
}
