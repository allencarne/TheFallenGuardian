using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasteOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHasteable hasteable = collision.GetComponent<IHasteable>();

        if (hasteable != null)
        {
            hasteable.Haste(Stacks, Duration);
        }
    }
}
