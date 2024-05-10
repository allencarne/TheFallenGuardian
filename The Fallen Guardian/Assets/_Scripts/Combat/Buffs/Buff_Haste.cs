using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Haste : MonoBehaviour, IHasteable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;

    [Header("Icon")]
    [SerializeField] GameObject buff_Haste;

    [Header("Haste")]
    int hasteStacks = 0;

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
        // Always stop the existing coroutine to ensure the current buff is reset
        StopCoroutine("HasteDuration");

        // Apply the new buff immediately, even if it's shorter than the current buff's remaining duration
        StartCoroutine(HasteDuration(stacks, duration));
    }

    IEnumerator HasteDuration(int stacks, float duration)
    {
        // Set Stacks
        hasteStacks = stacks;

        // Icon
        GameObject debuffIcon = Instantiate(buff_Haste);
        debuffIcon.transform.SetParent(buffBar.transform);
        debuffIcon.transform.localScale = new Vector3(1, 1, 1);

        // Logic for adjusting speed based on stacks
        float hasteAmount = 0.1f * hasteStacks; // Adjust this multiplier as needed
        ApplyHaste(hasteAmount);

        yield return new WaitForSeconds(duration);

        Destroy(debuffIcon);

        ResetHaste();
        hasteStacks = 0;
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
