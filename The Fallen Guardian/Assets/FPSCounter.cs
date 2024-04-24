using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;
    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Update the FPS text
        if (fpsText != null)
        {
            int fps = Mathf.RoundToInt(1.0f / deltaTime);
            fpsText.text = $"FPS: {fps}";
        }
    }
}
