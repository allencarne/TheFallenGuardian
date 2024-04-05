using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnTrigger : MonoBehaviour
{
    [HideInInspector] public float KnockBackForce;
    [HideInInspector] public float KnockBackDuration;
    [HideInInspector] public Vector2 KnockBackDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();

        if (knockbackable != null)
        {
            Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>();

            knockbackable.KnockBack(enemyRB, KnockBackForce, KnockBackDuration, KnockBackDirection);
        }
    }
}