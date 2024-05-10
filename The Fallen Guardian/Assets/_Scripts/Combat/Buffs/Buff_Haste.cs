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
    float speedPerStack = 3f;
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

                if (buffIcon) Destroy(buffIcon);
            }
        }
    }

    public void Haste(int stacks, float duration)
    {
        if (stacks > hasteStacks || !isHasted)
        {
            // Start or reset the haste buff
            hasteStacks = stacks;
            //currentDuration = duration;
            isHasted = true;

            ApplyHaste(stacks);

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
            stacksText.text = stacks.ToString();
        }

        if (duration > currentDuration)
        {
            // Update duration if it's larger
            currentDuration = duration;
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
