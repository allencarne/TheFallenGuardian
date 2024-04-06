using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    [Header("Attack")]
    [SerializeField] GameObject attackPrefab;
    [SerializeField] GameObject attackHitEffect;
    public int attackDamage;
    public float durationOfAttack;
    public float castTime;
    public float attackRange;
    public float attackCoolDown;

    protected override void AttackState()
    {
        if (crowdControl.isInterrupted)
        {
            //crowdControl.isInterrupted = false;
            enemyState = EnemyState.Idle;
            return;
        }

        if (canAttack)
        {
            canAttack = false;

            // Play attack animation
            enemyAnimator.Play("Attack");

            // Calculate the direction from the enemy to the target
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            // Set animator parameters based on the direction
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            StartCoroutine(CastTime(directionToTarget));
            StartCoroutine(DurationOfAttack());
        }
    }

    IEnumerator CastTime(Vector2 directionToTarget)
    {
        yield return new WaitForSeconds(castTime);

        if (enemyState == EnemyState.Attack)
        {
            // Calculate the position for the attackPrefab
            Vector2 attackPosition = (Vector2)transform.position + directionToTarget * attackRange;

            GameObject attack = Instantiate(attackPrefab, attackPosition, Quaternion.identity);

            // Ignore collision between the attack and the caster
            Physics2D.IgnoreCollision(attack.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

            DamageOnTrigger damageOnTrigger = attack.GetComponent<DamageOnTrigger>();
            if (damageOnTrigger != null)
            {
                damageOnTrigger.abilityDamage = attackDamage;
                damageOnTrigger.characterDamage = damage;
                damageOnTrigger.hitEffect = attackHitEffect;
            }
        }
    }

    IEnumerator DurationOfAttack()
    {
        yield return new WaitForSeconds(durationOfAttack);

        canAttack = true;
        if (enemyState == EnemyState.Attack)
        {
            enemyState = EnemyState.Idle;
        }
    }
}
