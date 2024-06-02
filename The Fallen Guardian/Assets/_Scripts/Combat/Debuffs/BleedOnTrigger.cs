using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IBleedable bleedable = collision.GetComponent<IBleedable>();

        if (bleedable != null)
        {
            bleedable.Bleed(Stacks, Duration);
        }
    }
}
