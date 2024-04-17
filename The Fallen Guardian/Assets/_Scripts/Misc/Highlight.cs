using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour
{
    Image image;
    float flashSpeed = 5;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        // Calculate the transparency value using a sine wave
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));

        // Set the alpha value of the image color
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}