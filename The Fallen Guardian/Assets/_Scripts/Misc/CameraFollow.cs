using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObjectRuntimeSet playerReference;
    public Transform playerTransform;
    public float smoothSpeed;

    public void OnPlayerJoin()
    {
        if (playerTransform == null)
        {
            playerTransform = playerReference.GetItemIndex(0).transform;
        }
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 desiredPosition = playerTransform.position;
            desiredPosition.z = transform.position.z;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
