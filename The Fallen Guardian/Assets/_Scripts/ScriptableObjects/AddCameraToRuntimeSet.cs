using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AddCameraToRuntimeSet : MonoBehaviour
{
    public UnityEvent OnCameraAdded;
    public CameraRuntimeSet cameraRuntimeSet;

    private void OnEnable()
    {
        cameraRuntimeSet.AddToList(gameObject);

        OnCameraAdded.Invoke();
    }

    private void OnDisable()
    {
        cameraRuntimeSet.RemoveFromList(gameObject);
    }
}
