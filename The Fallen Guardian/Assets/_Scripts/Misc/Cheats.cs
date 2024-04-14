using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cheats : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet PlayerReference;
    Player player;

    [SerializeField] PlayerStats stats;

    public void OnPlayerJoin()
    {
        Invoke("GetPlayer", .5f);
    }

    void GetPlayer()
    {
        if (PlayerReference.items.Count > 0)
        {
            player = PlayerReference.GetItemIndex(0).GetComponent<Player>();
        }
    }

    public void OnHealCheatPressed()
    {
        player.GetComponent<Testing>().CheatHeal();
    }
}
