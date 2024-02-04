using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [SerializeField] protected bool isInterruptable;
    protected bool isHurt;

    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float movementSpeed;

    protected event System.Action OnHurt;

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage Taken " + damage);

        isHurt = true;

        //OnHurt?.Invoke();
    }

    // Knockback
}
