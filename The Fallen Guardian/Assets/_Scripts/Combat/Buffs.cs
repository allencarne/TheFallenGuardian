using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;

    [Header("Buff")]
    [SerializeField] GameObject buff_Immovable;

    public bool IsImmovable;

    public void Immovable(float duration)
    {
        StartCoroutine(ImmovableDuration(duration));
    }

    IEnumerator ImmovableDuration(float duration)
    {
        IsImmovable = true;

        yield return new WaitForSeconds(duration);

        IsImmovable = false;
    }
}
