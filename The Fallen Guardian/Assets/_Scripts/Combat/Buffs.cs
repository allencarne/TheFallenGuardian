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
    float currentImmovableDuration = 0f;

    public void Immovable(float duration)
    {
        if (IsImmovable)
        {
            currentImmovableDuration += duration;
        }
        else
        {
            StartCoroutine(ImmovableDuration(duration));
        }
    }

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
}
