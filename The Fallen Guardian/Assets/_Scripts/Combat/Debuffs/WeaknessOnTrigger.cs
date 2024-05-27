using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IWeaknessable weaknessable = collision.GetComponent<IWeaknessable>();

        if (weaknessable != null)
        {
            weaknessable.Weakness(Stacks, Duration);
        }
    }
}
