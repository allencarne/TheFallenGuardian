using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    public float smoothSpeed;

    private void OnEnable()
    {
        GameManager.instance.OnPlayerJoin += HandlePlayerJoin;
    }

    private void OnDisable()
    {
        GameManager.instance.OnPlayerJoin -= HandlePlayerJoin;
    }

    void HandlePlayerJoin()
    {
        playerTransform = GameManager.instance.playerInstance.transform;
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Use Vector3.Lerp for smooth camera movement
            Vector3 desiredPosition = playerTransform.position;
            desiredPosition.z = transform.position.z; // Keep the camera's original z position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
