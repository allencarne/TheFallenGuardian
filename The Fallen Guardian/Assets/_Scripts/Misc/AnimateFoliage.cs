using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFoliage : MonoBehaviour
{
    [Header("Values")]
    public float minValue = -15f;
    public float maxValue = 15f;
    public float speed = 2f;

    [Header("Offset")]
    public float phaseOffset;

    float updateInterval = 1f / 24f; // Time interval for updating (24 frames per second)

    private void Start()
    {
        // Start the coroutine to update the rotation
        StartCoroutine(UpdateRotation());
    }

    private IEnumerator UpdateRotation()
    {
        while (true)
        {
            // Calculate the new rotation angle using a sine wave with a phase offset
            float angle = Mathf.Lerp(minValue, maxValue, (Mathf.Sin(Time.time * speed + phaseOffset) + 1f) / 2f);

            // Apply the rotation to the object
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Wait for the specified interval before updating again
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
