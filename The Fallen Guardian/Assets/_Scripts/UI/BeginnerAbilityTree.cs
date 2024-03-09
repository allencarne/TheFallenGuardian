using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerAbilityTree : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerReference;
    PlayerAbilities playerAbilities;

    [SerializeField] ScriptableObject frailSlash;

    [SerializeField] ScriptableObject lungeStrike;
    [SerializeField] ScriptableObject softSwing;

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

    public void OnFrailSlashSelected()
    {

    }
}
