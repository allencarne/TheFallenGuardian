using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    [SerializeField] int amountofDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if (collision.CompareTag("Player"))
            {

            }
            damageable.TakeDamage(amountofDamage);
        }
    }
}
