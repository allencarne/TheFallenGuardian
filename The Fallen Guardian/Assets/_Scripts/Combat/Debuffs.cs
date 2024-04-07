using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuffs : MonoBehaviour, ISlowable
{
    public bool IsSlowed;
    public float SlowAmount;

    public void Slow(float amount, float duration)
    {
        SlowAmount = amount;

        StartCoroutine(SlowDuration(duration));
    }

    IEnumerator SlowDuration(float duration)
    {
        IsSlowed = true;

        yield return new WaitForSeconds(duration);

        IsSlowed = false;
    }
}
