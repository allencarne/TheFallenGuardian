using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject HitEffect;
    [HideInInspector] public int CharacterDamage;
    [HideInInspector] public int AbilityDamage;

    [HideInInspector] public bool DestroyAfterDamage = false;
    [HideInInspector] public PlayerStateMachine stateMachine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(CharacterDamage + AbilityDamage);

            if (HitEffect)
            {
                Instantiate(HitEffect, collision.transform.position, collision.transform.rotation);
            }

            if (stateMachine)
            {
                stateMachine.Player.GainFury(5);  // Adjust the amount as needed
            }
        }

        // For Quest
        Dummy dummy = collision.GetComponent<Dummy>();
        if (dummy != null)
        {
            dummy.DummyDamaged();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if (DestroyAfterDamage)
            {
                Destroy(gameObject);
            }
        }
    }
}
