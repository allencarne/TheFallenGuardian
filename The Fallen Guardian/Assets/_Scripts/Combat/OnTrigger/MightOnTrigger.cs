using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MightOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IMightable mightable = collision.GetComponent<IMightable>();

        if (mightable != null)
        {
            mightable.Might(Stacks, Duration);
        }
    }
}
