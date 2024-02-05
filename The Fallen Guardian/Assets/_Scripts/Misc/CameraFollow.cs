using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] Camera player2Camera;
    public CameraRuntimeSet cameraReference;
    public GameObjectRuntimeSet playerReference;
    public float smoothSpeed;

    [Header("Transforms")]
    public Transform playerTransform;
    public Transform player2Transform;

    [Header("UI")]
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject player2UI;

    private void Start()
    {
        playerUI = GameObject.Find("PlayerUI");
    }

    public void HandlePlayerJoin()
    {
        if (playerTransform == null)
        {
            playerTransform = playerReference.GetItemIndex(0).transform;
        }
        else
        {
            // Set Tranforms
            player2Transform = playerReference.GetItemIndex(1).transform;

            // Spawn Second Camrea
            player2Camera = Instantiate(player2Camera);

            // Spawn Player2 UI
            player2UI = Instantiate(playerUI, playerUI.transform.parent);

            // Assign Player 2 UI to Player 2 Camera
            player2UI.GetComponent<Canvas>().worldCamera = player2Camera;

            // Set Player 1 Camrea viewport
            Camera mainCamera = Camera.main;
            mainCamera.rect = new Rect(0f, .5f, 1f, 1f);
        }
    }

    public void OnCameraAdded()
    {
        Debug.Log("CameraAdded");
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

        if (player2Transform != null)
        {
            Vector3 desiredPosition = player2Transform.position;
            desiredPosition.z = player2Camera.transform.position.z;
            Vector3 smoothedPosition = Vector3.Lerp(player2Camera.transform.position, desiredPosition, smoothSpeed);
            player2Camera.transform.position = smoothedPosition;
        }
    }
}
