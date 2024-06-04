using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IExhaustable exhaustable = collision.GetComponent<IExhaustable>();

        if (exhaustable != null)
        {
            exhaustable.Exhaust(Stacks, Duration);
        }
    }
}
