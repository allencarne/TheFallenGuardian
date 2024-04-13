using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;

    [Header("Buff")]
    [SerializeField] GameObject buff_Immovable;
    [SerializeField] GameObject buff_Regeneration;

    public bool IsImmovable;
    float currentImmovableDuration = 0f;

    public bool IsRegeneration;
    float currentRegenerationDuration = 0f;

    public void Immovable(float duration)
    {
        if (IsImmovable)
        {
            currentImmovableDuration += duration;
        }
        else
        {
            StartCoroutine(BuffDuration(duration, IsImmovable, currentImmovableDuration));
        }
    }
    /*
    IEnumerator ImmovableDuration(float duration)
    {
        IsImmovable = true;

        GameObject buffIcon = Instantiate(buff_Immovable);
        buffIcon.transform.SetParent(buffBar.transform);
        buffIcon.transform.localScale = new Vector3(1,1,1);

        yield return new WaitForSeconds(duration);

        Destroy(buffIcon);

        IsImmovable = false;

        currentImmovableDuration = 0;
    }
    */
    public void Regeneration(float amount, float duration)
    {
        if (IsRegeneration)
        {
            currentRegenerationDuration += duration;
        }
        else
        {
            StartCoroutine(BuffDuration(duration, IsRegeneration, currentRegenerationDuration));
        }
    }

    IEnumerator BuffDuration(float duration, bool buffBool, float currentBuffDuration)
    {
        buffBool = true;

        GameObject buffIcon = Instantiate(buff_Immovable);
        buffIcon.transform.SetParent(buffBar.transform);
        buffIcon.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(duration);

        Destroy(buffIcon);

        buffBool = true;

        currentBuffDuration = 0;
    }
}
