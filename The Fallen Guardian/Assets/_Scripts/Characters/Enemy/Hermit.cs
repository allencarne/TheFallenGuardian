using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermit : Enemy
{
    #region Basic

    [Header("Basic")]
    [SerializeField] GameObject basicTelegraph;
    GameObject basicTelegraphInstance;

    [SerializeField] GameObject basicPrefab;
    [SerializeField] GameObject basicHitEffect;

    [Header("Stats")]
    [SerializeField] int basicDamage;
    [SerializeField] float basicRange;

    [Header("Time")]
    [SerializeField] float basicCastTime;
    [SerializeField] float basicRecoveryTime;
    [SerializeField] float basicCoolDown;

    bool wasInterrupted = false;

    protected override void AttackState()
    {
        modifiedCastTime = basicCastTime / CurrentAttackSpeed;

        if (castBar.color == Color.yellow)
        {
            // Increase cast bar time once per second
            castBarTime += Time.deltaTime;

            // If we are not interrupted, Update the cast bar
            UpdateCastBar(castBarTime, modifiedCastTime);
        }

        if (crowdControl.IsInterrupted)
        {
            wasInterrupted = true;

            if (basicTelegraphInstance)
            {
                Destroy(basicTelegraphInstance);
            }

            if (castBar.color != Color.green)
            {
                // ResetCast Time
                castBarTime = 0;

                // Set Cast Bar Color
                castBar.color = Color.red;

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

            // Set Cast Bar Color
            castBar.color = Color.yellow;

            // Calculate the direction from the enemy to the target
            directionToTarget = (target.position - transform.position).normalized;

            // Calculate the position for the basicPrefab
            vectorToTarget = (Vector2)transform.position + directionToTarget * basicRange;

            // Play attack animation
            enemyAnimator.Play("Basic Cast");
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            // Instantiate the telegraph object at the enemy position with the appropriate rotation
            basicTelegraphInstance = Instantiate(basicTelegraph, vectorToTarget, Quaternion.identity, transform);

            FillTelegraph fillTelegraph = basicTelegraphInstance.GetComponent<FillTelegraph>();
            if (fillTelegraph != null)
            {
                fillTelegraph.FillSpeed = modifiedCastTime;
            }

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
        UpdateCastBar(castBarTime, modifiedCastTime);

        yield return new WaitForSeconds(modifiedCastTime);

        if (!wasInterrupted)
        {
            // Animate
            enemyAnimator.Play("Basic Impact");

            // Set Cast Bar Color
            castBar.color = Color.green;

            GameObject basic = Instantiate(basicPrefab, vectorToTarget, Quaternion.identity, transform);

            // Cannot Hit Self with Attack
            Physics2D.IgnoreCollision(basic.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

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
        else
        {
            wasInterrupted = false;

            hasAttacked = false;
        }
    }

    IEnumerator ImpactDelay()
    {
        yield return new WaitForSeconds(.1f);

        canImpact = true;

        castBarTime = 0;
        StartCoroutine(EndCastBar());
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

    #region Mobility

    [Header("Mobility")]
    [SerializeField] GameObject mobilityPrefab;
    [SerializeField] GameObject mobilityHitEffect;
    [SerializeField] GameObject mobilitTelegraph;

    [Header("Stats")]
    [SerializeField] int mobilityDamage;
    [SerializeField] float mobilityRange;

    [Header("Time")]
    [SerializeField] float mobilityCastTime;
    [SerializeField] float mobilityRecoveryTime;
    [SerializeField] float mobilityCoolDown;



    #endregion

    #region Special

    [Header("Special")]
    [SerializeField] GameObject specialPrefab;
    [SerializeField] GameObject specialHitEffect;
    [SerializeField] GameObject specialTelegraph;

    [Header("Stats")]
    [SerializeField] int specialDamage;

    [Header("Time")]
    [SerializeField] float specialCastTime;
    [SerializeField] float specialImpactTime;
    [SerializeField] float specialRecoveryTime;
    [SerializeField] float specialCoolDown;
    [SerializeField] float specialAttackRate;

    protected override void SpecialState()
    {
        modifiedCastTime = specialCastTime / CurrentAttackSpeed;

        if (castBar.color == Color.yellow)
        {
            // Increase cast bar time once per second
            castBarTime += Time.deltaTime;

            // If we are not interrupted, Update the cast bar
            UpdateCastBar(castBarTime, modifiedCastTime);
        }

        if (crowdControl.IsInterrupted)
        {
            if (castBar.color != Color.green)
            {
                // ResetCast Time
                castBarTime = 0;

                // Set Cast Bar Color
                castBar.color = Color.red;

                // State Transition
                enemyState = EnemyState.Idle;

                StartCoroutine(ResetCastBar());
                return;
            }
        }

        if (canSpecial && target != null && !hasAttacked)
        {
            hasAttacked = true;
            canSpecial = false;

            // Set Cast Bar Color
            castBar.color = Color.yellow;

            // Play attack animation
            enemyAnimator.Play("Special Cast");

            // Instantiate the telegraph object at the enemy position with the appropriate rotation
            GameObject telegraph = Instantiate(specialTelegraph, transform.position, Quaternion.identity, transform);

            FillTelegraph fillTelegraph = telegraph.GetComponent<FillTelegraph>();
            if (fillTelegraph != null)
            {
                fillTelegraph.FillSpeed = modifiedCastTime;
            }

            // Timers
            StartCoroutine(SpecialImpact());
            StartCoroutine(SpecialCoolDown());
        }

        if (canImpact)
        {
            canImpact = false;

            StartCoroutine(SpecialRecoveryTime());
        }
    }

    IEnumerator SpecialImpact()
    {
        UpdateCastBar(castBarTime, modifiedCastTime);

        // Wait for the modifiedCastTime duration
        yield return new WaitForSeconds(modifiedCastTime);

        // Animate
        enemyAnimator.Play("Special Impact");

        // Set Cast Bar Color
        castBar.color = Color.green;

        // Delay
        StartCoroutine(SpecialImpactDelay());
    }

    IEnumerator SpecialImpactDelay()
    {
        // Start invoking the SpawnSpecialPrefab method every second
        InvokeRepeating("SpawnSpecialPrefab", 0f, specialAttackRate);

        yield return new WaitForSeconds(specialImpactTime);

        // Stop invoking the SpawnSpecialPrefab method
        CancelInvoke("SpawnSpecialPrefab");

        canImpact = true;

        castBarTime = 0;
        StartCoroutine(EndCastBar());
    }

    void SpawnSpecialPrefab()
    {
        GameObject special = Instantiate(specialPrefab, transform.position, Quaternion.identity, transform);

        DamageOnTrigger damageOnTrigger = special.GetComponent<DamageOnTrigger>();
        if (damageOnTrigger != null)
        {
            damageOnTrigger.AbilityDamage = specialDamage;
            damageOnTrigger.CharacterDamage = CurrentDamage;
            damageOnTrigger.HitEffect = specialHitEffect;
        }
    }

    IEnumerator SpecialRecoveryTime()
    {
        // Animate
        enemyAnimator.Play("Special Recovery");

        float modifiedRecoveryTime = specialRecoveryTime / CurrentAttackSpeed;

        yield return new WaitForSeconds(modifiedRecoveryTime);

        hasAttacked = false;

        enemyState = EnemyState.Idle;
    }

    IEnumerator SpecialCoolDown()
    {
        // Adjust cooldown time based on cooldown reduction
        float modifiedCooldown = specialCoolDown / CurrentCDR;

        yield return new WaitForSeconds(modifiedCooldown);

        canSpecial = true;
    }

    #endregion
}
