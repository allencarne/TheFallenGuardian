using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;

    public void OnPauseUIOpened()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }
}
