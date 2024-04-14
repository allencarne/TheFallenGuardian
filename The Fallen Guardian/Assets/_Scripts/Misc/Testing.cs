using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Testing : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = .75f;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Time.timeScale = .5f;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            Time.timeScale = .25f;
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Time.timeScale = 0f;
        }
    }

    public void CheatHeal()
    {
        player.Heal(1);
    }
}
