using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftnessOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ISwiftnessable swiftnessable = collision.GetComponent<ISwiftnessable>();

        if (swiftnessable != null)
        {
            swiftnessable.Swiftness(Stacks, Duration);
        }
    }
}
