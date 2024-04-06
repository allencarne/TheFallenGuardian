using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        float remainingCastTime = castTime;
        while (remainingCastTime > 0)
        {
            if (crowdControl.isInterrupted)
            {
                // Change color to red briefly
                castBar.color = Color.red;
                yield return new WaitForSeconds(0.2f); // Brief pause for visibility
                castBar.color = Color.yellow;
                UpdateCastBar(0f); // Reset the cast bar
                yield break; // Exit the coroutine
            }

            remainingCastTime -= Time.deltaTime;
            float progress = 1 - (remainingCastTime / castTime);
            UpdateCastBar(progress); // Update the cast bar
            yield return null;
        }

        // Cast successful, change color to green briefly
        castBar.color = Color.green;
        yield return new WaitForSeconds(0.2f); // Brief pause for visibility
        castBar.color = Color.yellow;
        UpdateCastBar(0f); // Reset the cast bar

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
