using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermit : Enemy
{
    #region Basic

    [Header("Basic")]
    [SerializeField] GameObject basicPrefab;
    [SerializeField] GameObject basicHitEffect;
    [SerializeField] GameObject basicTelegraph;

    [Header("Stats")]
    [SerializeField] int basicDamage;
    [SerializeField] float basicRange;

    [Header("Time")]
    [SerializeField] float basicCastTime;
    [SerializeField] float basicRecoveryTime;
    [SerializeField] float basicCoolDown;

    protected override void AttackState()
    {
        if (crowdControl.IsInterrupted)
        {
            if (castBar.color != Color.green)
            {
                // ResetCast Time
                //castTime = 0;

                // Set Cast Bar Color
                //castBar.color = Color.red;

                // State Transition
                enemyState = EnemyState.Idle;

                StartCoroutine(ResetCastBar());
                return;
            }
        }

        if (canAttack && target != null && !hasAttacked)
        {
            hasAttacked = true;
            canAttack = false;

            // Calculate the direction from the enemy to the target
            directionToTarget = (target.position - transform.position).normalized;

            // Calculate the angle in degrees from the direction
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Play attack animation
            enemyAnimator.Play("Basic Cast");
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            // Instantiate the telegraph object at the enemy position with the appropriate rotation
            Instantiate(basicTelegraph, transform.position, Quaternion.Euler(0f, 0f, angle), transform);

            // Timers
            StartCoroutine(BasicImpact());
            StartCoroutine(AttackCoolDown());
        }

        if (canImpact)
        {
            canImpact = false;

            StartCoroutine(RecoveryTime());
        }
    }

    IEnumerator BasicImpact()
    {
        float modifiedCastTime = basicCastTime / CurrentAttackSpeed;

        yield return new WaitForSeconds(modifiedCastTime);

        // Animate
        enemyAnimator.Play("Basic Impact");

        // Calculate the position for the basicPrefab
        Vector2 basicPosition = (Vector2)transform.position + directionToTarget * basicRange;

        GameObject basic = Instantiate(basicPrefab, basicPosition, Quaternion.identity);

        DamageOnTrigger damageOnTrigger = basic.GetComponent<DamageOnTrigger>();
        if (damageOnTrigger != null)
        {
            damageOnTrigger.AbilityDamage = basicDamage;
            damageOnTrigger.CharacterDamage = CurrentDamage;
            damageOnTrigger.HitEffect = basicHitEffect;
        }

        // Delay
        StartCoroutine(ImpactDelay());
    }

    IEnumerator ImpactDelay()
    {
        yield return new WaitForSeconds(.1f);

        canImpact = true;
    }

    IEnumerator RecoveryTime()
    {
        // Animate
        enemyAnimator.Play("Basic Recovery");

        float modifiedRecoveryTime = basicRecoveryTime / CurrentAttackSpeed;

        yield return new WaitForSeconds(modifiedRecoveryTime);

        hasAttacked = false;

        enemyState = EnemyState.Idle;
    }

    IEnumerator AttackCoolDown()
    {
        // Adjust cooldown time based on cooldown reduction
        float modifiedCooldown = basicCoolDown / CurrentCDR;

        yield return new WaitForSeconds(modifiedCooldown);

        canAttack = true;
    }

    #endregion
}
