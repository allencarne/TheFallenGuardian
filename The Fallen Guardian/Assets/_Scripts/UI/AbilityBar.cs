using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    [SerializeField] BeginnerAbilityTree AbilityTree;

    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerAbilities playerAbilities;

    [SerializeField] Image Ability1;
    [SerializeField] Image Ability2;
    [SerializeField] Image Ability3;
    [SerializeField] Image Ability4;
    [SerializeField] Image Ability5;
    [SerializeField] Image Ability6;

    public void OnPlayerJoin()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);

        if (playerReference.items.Count > 0)
        {
            playerAbilities = playerReference.GetItemIndex(0).GetComponent<PlayerAbilities>();
        }
    }

    public void UpdateAbility1()
    {
        Ability1.enabled = true;
        Ability1.sprite = AbilityTree.level1AbilityImage.sprite;
    }
}
