using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AddGameObjectToRuntimeSet : MonoBehaviour
{
    public UnityEvent OnObjectAdded;
    public GameObjectRuntimeSet gameObjectRuntimeSet;

    private void OnEnable()
    {
        gameObjectRuntimeSet.AddToList(gameObject);

        OnObjectAdded.Invoke();
    }

    private void OnDisable()
    {
        gameObjectRuntimeSet.RemoveFromList(gameObject);
    }
}
