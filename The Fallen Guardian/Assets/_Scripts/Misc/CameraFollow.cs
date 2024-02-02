using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;

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
            Vector3 playerPos = playerTransform.position;
            playerPos.z = transform.position.z;
            transform.position = playerPos;
        }
    }
}
