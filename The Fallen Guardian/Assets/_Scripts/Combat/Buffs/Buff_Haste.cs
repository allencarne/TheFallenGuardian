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
    public bool IsHasted;
    float currentHasteDuration = 0f;

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

    public void Haste(float amount, float duration)
    {
        if (IsHasted)
        {
            currentHasteDuration += duration;
        }
        else
        {
            StartCoroutine(HasteDuration(amount, duration));
        }
    }

    IEnumerator HasteDuration(float amount, float duration)
    {
        // Bool
        IsHasted = true;

        // Icon
        GameObject debuffIcon = Instantiate(buff_Haste);
        debuffIcon.transform.SetParent(buffBar.transform);
        debuffIcon.transform.localScale = new Vector3(1, 1, 1);

        // Apply haste
        ApplyHaste(amount);

        yield return new WaitForSeconds(duration);

        // Reset haste
        ResetHaste(amount);

        Destroy(debuffIcon);

        currentHasteDuration = 0f;

        IsHasted = false;
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

    void ResetHaste(float amount)
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
