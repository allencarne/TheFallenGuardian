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

    [Header("Haste")]
    int mightPerStack = 3;
    bool isMighted = false;
    float currentDuration = 0;
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

    private void Update()
    {
        // If haste buff is active, decrement the duration
        if (isMighted)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration <= 0)
            {
                EndMight();

                if (buffIcon) Destroy(buffIcon);
            }
        }
    }

    public void Might(int stacks, float duration)
    {
        if (stacks > mightStacks || !isMighted)
        {
            // Start or reset the haste buff
            mightStacks = stacks;
            //currentDuration = duration;
            isMighted = true;

            ApplyMight(stacks);

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
            stacksText.text = stacks.ToString();
        }

        if (duration > currentDuration)
        {
            // Update duration if it's larger
            currentDuration = duration;
        }
    }

    void ApplyMight(int stacks)
    {
        // Calculate the haste amount based on the number of stacks
        int mightAmount = mightPerStack * stacks; // Increase movement speed by 1 for each stack

        CurrentDamage = BaseDamage + mightAmount;

        if (isPlayer)
        {
            playerStats.CurrentDamage = CurrentDamage;
        }
        else
        {
            enemy.CurrentDamage = CurrentDamage;
        }
    }

    public void EndMight()
    {
        // Reset haste-related variables
        isMighted = false;
        mightStacks = 0;
        currentDuration = 0f;

        // Reset the speed
        ResetMight();
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
