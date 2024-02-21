using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnTrigger : MonoBehaviour
{
    [SerializeField] int amountofknockback;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();

        if (knockbackable != null)
        {
            Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>();

            knockbackable.KnockBack(collision.transform.position, transform.position, enemyRB, amountofknockback);
        }
    }
}
