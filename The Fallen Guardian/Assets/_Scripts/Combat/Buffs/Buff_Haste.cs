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
    [SerializeField] TextMeshProUGUI stacksText;

    [Header("Haste")]
    bool isHasted = false;
    float currentDuration = 0;
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

        // Get Stacks Text
        stacksText = buff_Haste.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        // If haste buff is active, decrement the duration
        if (isHasted)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration <= 0)
            {
                EndHaste();

                if (buffIcon)
                {
                    Destroy(buffIcon);
                }
            }
        }
    }

    public void Haste(int stacks, float duration)
    {
        if (stacks > hasteStacks || !isHasted)
        {
            // Start or reset the haste buff
            hasteStacks = stacks;
            currentDuration = duration;
            isHasted = true;

            // Apply haste effect on speed
            float hasteAmount = 0.1f * hasteStacks; // Adjust this multiplier as needed
            ApplyHaste(hasteAmount);

            // Icon
            if (!buffIcon)
            {
                buffIcon = Instantiate(buff_Haste);
                buffIcon.transform.SetParent(buffBar.transform);
                buffIcon.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        // Stacks Text
        stacksText.text = stacks.ToString();
    }

    void ApplyHaste(float amount)
    {
        CurrentSpeed = Mathf.Max(BaseSpeed + amount, 0);

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
        // Reset haste-related variables
        isHasted = false;
        hasteStacks = 0;
        currentDuration = 0f;

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
