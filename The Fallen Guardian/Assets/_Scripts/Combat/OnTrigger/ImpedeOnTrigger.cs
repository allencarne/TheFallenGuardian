using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpedeOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IImpedable impedable = collision.GetComponent<IImpedable>();

        if (impedable != null)
        {
            impedable.Impede(Stacks, Duration);
        }
    }
}
