using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasteOnTrigger : MonoBehaviour
{
    public float HasteAmount;
    public float HasteDuration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHasteable hasteable = collision.GetComponent<IHasteable>();

        if (hasteable != null)
        {
            hasteable.Haste(HasteAmount, HasteDuration);
        }
    }
}
