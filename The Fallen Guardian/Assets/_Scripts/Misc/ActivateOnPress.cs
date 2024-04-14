using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnPress : MonoBehaviour
{
    [SerializeField] GameObject gObject;

    public void Press()
    {
        gObject.SetActive(!gObject.activeSelf);
    }
}
