using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Debuffs : MonoBehaviour, ISlowable
{
    [Header("Debuff Bar")]
    [SerializeField] GameObject debuffBar;

    [Header("Debuff")]
    [SerializeField] GameObject debuff_Slow;

    [Header("Slow")]
    public bool IsSlowed;
    public UnityEvent OnSlowed;
    public UnityEvent OnSlowEnd;
    public float SlowAmount;
    float currentSlowDuration = 0f;

    public void Slow(float amount, float duration)
    {
        SlowAmount = amount;

        if (IsSlowed)
        {
            currentSlowDuration += duration;
        }
        else
        {
            StartCoroutine(SlowDuration(duration));
        }
    }

    IEnumerator SlowDuration(float duration)
    {
        OnSlowed?.Invoke();

        IsSlowed = true;

        GameObject debuffIcon = Instantiate(debuff_Slow);
        debuffIcon.transform.SetParent(debuffBar.transform);
        debuffIcon.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(duration);

        Destroy(debuffIcon);

        OnSlowEnd?.Invoke();

        IsSlowed = false;

        currentSlowDuration = 0f;
    }
}
