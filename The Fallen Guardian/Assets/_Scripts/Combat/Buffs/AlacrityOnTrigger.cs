using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlacrityOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IAlacrityable alacrityable = collision.GetComponent<IAlacrityable>();

        if (alacrityable != null)
        {
            alacrityable.Alacrity(Stacks, Duration);
        }
    }
}