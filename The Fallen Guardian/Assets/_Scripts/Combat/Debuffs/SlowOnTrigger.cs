using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnTrigger : MonoBehaviour
{
    public int SlowAmount;
    public float SlowDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ISlowable slowable = collision.GetComponent<ISlowable>();

        if (slowable != null)
        {
            slowable.Slow(SlowAmount, SlowDuration);
        }
    }
}
