using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject hitEffect;
    [HideInInspector] public int playerDamage;
    [HideInInspector] public int abilityDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(playerDamage + abilityDamage);

            if (hitEffect)
            {
                Instantiate(hitEffect, collision.transform.position, collision.transform.rotation);
            }
        }
    }
}
