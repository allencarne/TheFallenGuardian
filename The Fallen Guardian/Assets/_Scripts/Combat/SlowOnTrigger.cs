using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnTrigger : MonoBehaviour
{
    [HideInInspector] public float SlowAmount;
    [HideInInspector] public float SlowDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ISlowable slowable = collision.GetComponent<ISlowable>();

        if (slowable != null)
        {
            Debug.Log("Slow");
            slowable.Slow(SlowAmount, SlowDuration);
        }
    }
}
