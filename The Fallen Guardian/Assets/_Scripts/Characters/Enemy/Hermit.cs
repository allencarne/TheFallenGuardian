using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermit : Enemy
{
    [Header("Variables")]
    float castTime = 0;
    bool canImpact = true;
    bool canRecovery = false;
    Vector2 directionToTarget;
    //Vector2 dashDirection;

    [Header("Attack")]
    [SerializeField] GameObject basicPrefab;
    [SerializeField] GameObject basicHitEffect;
    [SerializeField] GameObject basicTelegraph;
    public int basicDamage;
    public float basicCastTime;
    public float basicRange;
    public float basicCoolDown;

    protected override void AttackState()
    {
        // Adjust the cast time based on attack speed
        float modifiedAttackCastTime = basicCastTime / CurrentAttackSpeed;

        castTime += Time.deltaTime;
        UpdateCastBar(castTime, CurrentAttackSpeed);

        if (crowdControl.IsInterrupted)
        {
            if (castBar.color != Color.green)
            {
                // ResetCast Time
                castTime = 0;

                // Set Cast Bar Color
                castBar.color = Color.red;

                // State Transition
                enemyState = EnemyState.Idle;

                StartCoroutine(ResetCastBar());
                return;
            }
        }

        if (canAttack && target != null)
        {
            canAttack = false;

            castBar.color = Color.yellow;

            // Play attack animation
            enemyAnimator.Play("Attack Cast");

            // Calculate the direction from the enemy to the target
            directionToTarget = (target.position - transform.position).normalized;

            // Calculate the angle in degrees from the direction
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Instantiate the telegraph object at the enemy position with the appropriate rotation
            Instantiate(basicTelegraph, transform.position, Quaternion.Euler(0f, 0f, angle), transform);

            // Set animator parameters based on the direction
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            // Start Cooldown
            StartCoroutine(AttackCoolDown());
        }

        if (castTime > modifiedAttackCastTime)
        {
            if (canImpact)
            {
                canImpact = false;

                castBar.color = Color.green;
                enemyAnimator.Play("Attack Impact");

                // Calculate the position for the attackPrefab
                Vector2 attackPosition = (Vector2)transform.position + directionToTarget * basicRange;

                GameObject attack = Instantiate(basicPrefab, attackPosition, Quaternion.identity);

                DamageOnTrigger damageOnTrigger = attack.GetComponent<DamageOnTrigger>();
                if (damageOnTrigger != null)
                {
                    damageOnTrigger.AbilityDamage = basicDamage;
                    damageOnTrigger.CharacterDamage = CurrentDamage;
                    damageOnTrigger.HitEffect = basicHitEffect;
                }
            }
        }

        if (canRecovery)
        {
            canRecovery = false;

            enemyAnimator.Play("Attack Recovery");
        }
    }

    public void AE_EndOfImpact()
    {
        canRecovery = true;
    }

    public void AE_EndOfRecovery()
    {
        canImpact = true;

        castBar.color = Color.yellow;
        castTime = 0;
        UpdateCastBar(0, basicCastTime);

        // State Transition
        enemyState = EnemyState.Idle;
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(basicCoolDown);

        canAttack = true;
    }
}
