using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Buff_Immovable : MonoBehaviour, IImmovable
{
    [Header("Buff Bar")]
    [SerializeField] GameObject buffBar;
    [SerializeField] GameObject ImmovableEffect;
    GameObject ImmovableEffectInstance;

    [Header("Icon")]
    [SerializeField] GameObject buffPrefab;
    GameObject buffIcon;

    public bool isImmovable = false;

    public void Immovable(float Duration)
    {
        isImmovable = true;

        // Icon
        if (!buffIcon)
        {
            buffIcon = Instantiate(buffPrefab, buffBar.transform);
            buffIcon.transform.localScale = new Vector3(1, 1, 1);
        }

        if (!ImmovableEffectInstance)
        {
            ImmovableEffectInstance = Instantiate(ImmovableEffect, transform);
        }

        StartCoroutine(ImmovableDuration(Duration));
    }

    IEnumerator ImmovableDuration(float Duration)
    {
        yield return new WaitForSeconds(Duration);

        isImmovable = false;

        Destroy(buffIcon);
        Destroy(ImmovableEffectInstance);
    }
}
