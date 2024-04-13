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
}
