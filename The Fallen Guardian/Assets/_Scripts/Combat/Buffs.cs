using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buffs : MonoBehaviour, IHasteable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;

    [Header("Buff")]
    [SerializeField] GameObject buff_Immovable;
    [SerializeField] GameObject buff_Regeneration;

    [Header("Haste")]
    public bool IsHasted;
    public UnityEvent OnHasted;
    public UnityEvent OnHasteEnd;
    public float HasteAmount;
    float currentHasteDuration = 0f;

    public bool IsImmovable;
    float currentImmovableDuration = 0f;

    public bool IsRegeneration;
    private GameObject currentRegenerationIcon;

    public void Immovable(float duration)
    {
        if (IsImmovable)
        {
            currentImmovableDuration += duration;
        }
        else
        {
            StartCoroutine(BuffDuration(duration, IsImmovable, currentImmovableDuration, buff_Immovable));
        }
    }

    public void Regeneration()
    {
        if (IsRegeneration && currentRegenerationIcon == null)
        {
            currentRegenerationIcon = Instantiate(buff_Regeneration);
            currentRegenerationIcon.transform.SetParent(buffBar.transform);
            currentRegenerationIcon.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (!IsRegeneration && currentRegenerationIcon != null)
        {
            Destroy(currentRegenerationIcon);
            currentRegenerationIcon = null;
        }
    }

    IEnumerator BuffDuration(float duration, bool buffBool, float currentBuffDuration, GameObject prefab)
    {
        buffBool = true;

        GameObject buffIcon = Instantiate(prefab);
        buffIcon.transform.SetParent(buffBar.transform);
        buffIcon.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(duration);

        Destroy(buffIcon);

        buffBool = true;

        currentBuffDuration = 0;
    }

    public void Haste(float amount, float duration)
    {
        HasteAmount = amount;

        if (IsHasted)
        {
            currentHasteDuration += duration;
        }
        else
        {
            StartCoroutine(SlowDuration(duration));
        }
    }

    IEnumerator SlowDuration(float duration)
    {
        OnHasted?.Invoke();

        IsHasted = true;

        //GameObject debuffIcon = Instantiate(debuff_Slow);
        //debuffIcon.transform.SetParent(debuffBar.transform);
        //debuffIcon.transform.localScale = new Vector3(1, 1, 1);

        yield return new WaitForSeconds(duration);

        //Destroy(debuffIcon);

        OnHasteEnd?.Invoke();

        IsHasted = false;

        currentHasteDuration = 0f;
    }
}
