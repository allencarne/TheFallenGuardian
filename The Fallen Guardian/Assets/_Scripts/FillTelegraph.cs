using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillTelegraph : MonoBehaviour
{
    [SerializeField] SpriteRenderer frontSprite;
    public float FillSpeed;

    private void Update()
    {
        // Calculate the scale increment per frame to achieve the target scale in FillSpeed seconds
        float scaleIncrement = Time.deltaTime / FillSpeed;

        // Adjust the scale of frontSprite
        Vector3 currentScale = frontSprite.transform.localScale;
        float newScaleX = Mathf.Min(currentScale.x + scaleIncrement, 1f);
        float newScaleY = Mathf.Min(currentScale.y + scaleIncrement, 1f);

        // Set the new scale
        frontSprite.transform.localScale = new Vector3(newScaleX, newScaleY, currentScale.z);

        // Check if the scale has reached 1
        if (frontSprite.transform.localScale.x >= 1f && frontSprite.transform.localScale.y >= 1f)
        {
            // Destroy this game object
            Destroy(gameObject);
        }
    }
}
