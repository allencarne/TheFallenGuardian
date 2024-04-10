using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] animationFrames;
    Image imageComponent;

    float frameRate = 0.2f; // Change this value to adjust animation speed
    int currentFrameIndex = 0;

    void Start()
    {
        imageComponent = GetComponent<Image>();

        // Check if there are frames in the animation
        if (animationFrames.Length == 0)
        {
            Debug.LogError("No frames assigned for animation.");
            enabled = false; // Disable the script if there are no frames
        }
        else
        {
            // Set the initial frame
            imageComponent.sprite = animationFrames[currentFrameIndex];

            // Start the animation
            InvokeRepeating("AnimateNextFrame", frameRate, frameRate);
        }
    }

    void AnimateNextFrame()
    {
        // Increment frame index
        currentFrameIndex = (currentFrameIndex + 1) % animationFrames.Length;

        // Update the image component with the new frame
        imageComponent.sprite = animationFrames[currentFrameIndex];
    }
}
