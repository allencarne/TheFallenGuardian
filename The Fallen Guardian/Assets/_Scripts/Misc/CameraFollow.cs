using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Data")]
    PlayerData playerData;
    public float smoothSpeed;

    [Header("Transforms")]
    private Transform playerTransform;
    private Transform player2Transform;

    [Header("UI")]
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject player2UI;

    private void Start()
    {
        playerUI = GameObject.Find("PlayerUI");
    }

    public void HandlePlayerJoin()
    {
        Debug.Log("test");
        //playerTransform = playerData.playerInstanceName;
        //playerTransform = GameManager.instance.playerInstance.transform;
    }

    public void HandlePlayer2Join()
    {
        // Spawn Second Camrea
        GameManager.instance.player2Camera = Instantiate(GameManager.instance.player2Camera);

        // Set Player 1 Camrea viewport rect y = .5
        Camera mainCamera = Camera.main;
        mainCamera.rect = new Rect(0f, .5f, 1f, 1f);

        // Set Tranforms to Player2 Instance
        //player2Transform = GameManager.instance.player2Instance.transform;

        // Spawn Player2 UI
        player2UI = Instantiate(playerUI, playerUI.transform.parent);

        // Assign Player 2 UI to Player 2 Camera
        player2UI.GetComponent<Canvas>().worldCamera = GameManager.instance.player2Camera;
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
            desiredPosition.z = GameManager.instance.player2Camera.transform.position.z;
            Vector3 smoothedPosition = Vector3.Lerp(GameManager.instance.player2Camera.transform.position, desiredPosition, smoothSpeed);
            GameManager.instance.player2Camera.transform.position = smoothedPosition;
        }
    }
}
