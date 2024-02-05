using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGameObjectToRuntimeSet : MonoBehaviour
{
    public GameObjectRuntimeSet gameObjectRuntimeSet;

    private void OnEnable()
    {
        gameObjectRuntimeSet.AddToList(gameObject);
        Debug.Log(gameObject.name + " added to the list.");
    }

    private void OnDisable()
    {
        gameObjectRuntimeSet.RemoveFromList(gameObject);
    }
}
