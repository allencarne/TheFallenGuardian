using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ISlowable slowable = collision.GetComponent<ISlowable>();

        if (slowable != null)
        {
            slowable.Slow(Stacks, Duration);
        }
    }
}
