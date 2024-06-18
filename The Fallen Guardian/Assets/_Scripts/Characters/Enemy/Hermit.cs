using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hermit : Enemy
{

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (enemyState == EnemyState.Mobility && target != null)
        {
            if (canDash)
            {
                // Disable collision between enemy and player
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), true);

                // Interpolate the enemy's position towards the target
                transform.position = Vector2.Lerp(transform.position, vectorToTarget, Time.fixedDeltaTime * 5);
            }
        }
    }

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
    [SerializeField] float basicImpactTime;
    [SerializeField] float basicRecoveryTime;
    [SerializeField] float basicCoolDown;

    protected override void AttackState()
    {
        modifiedCastTime = basicCastTime / CurrentAttackSpeed;
        modifiedImpactTime = basicImpactTime / CurrentAttackSpeed;
        modifiedRecoveryTime = basicRecoveryTime / CurrentAttackSpeed;

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
            canAttack = false;
            hasAttacked = true;

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

            // Instantiate the telegraph
            basicTelegraphInstance = Instantiate(basicTelegraph, vectorToTarget, Quaternion.identity);

            FillTelegraph _fillTelegraph = basicTelegraphInstance.GetComponent<FillTelegraph>();
            if (_fillTelegraph != null)
            {
                _fillTelegraph.FillSpeed = modifiedCastTime;
            }

            // Timers
            StartCoroutine(BasicCast());
            StartCoroutine(AttackCoolDown());
        }
    }

    IEnumerator BasicCast()
    {
        yield return new WaitForSeconds(modifiedCastTime);

        if (!wasInterrupted)
        {
            // Animate
            enemyAnimator.Play("Basic Impact");

            // Set Cast Bar Color
            castBar.color = Color.green;

            GameObject _basic = Instantiate(basicPrefab, vectorToTarget, Quaternion.identity);

            // Cannot Hit Self with Attack
            Physics2D.IgnoreCollision(_basic.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

            DamageOnTrigger _damageOnTrigger = _basic.GetComponent<DamageOnTrigger>();
            if (_damageOnTrigger != null)
            {
                _damageOnTrigger.AbilityDamage = basicDamage;
                _damageOnTrigger.CharacterDamage = CurrentDamage;
                _damageOnTrigger.HitEffect = basicHitEffect;
            }

            // Delay
            StartCoroutine(BasicImpact());
        }
        else
        {
            wasInterrupted = false;
            hasAttacked = false;
        }
    }

    IEnumerator BasicImpact()
    {
        yield return new WaitForSeconds(modifiedImpactTime);

        // Cast bar
        castBarTime = 0;
        StartCoroutine(EndCastBar());

        // Animate
        enemyAnimator.Play("Basic Recovery");

        StartCoroutine(BasicRecovery());
    }

    IEnumerator BasicRecovery()
    {
        yield return new WaitForSeconds(modifiedRecoveryTime);

        wasInterrupted = false;
        hasAttacked = false;

        enemyState = EnemyState.Idle;
    }

    IEnumerator AttackCoolDown()
    {
        // Adjust cooldown time based on cooldown reduction
        float _modifiedCooldown = basicCoolDown / CurrentCDR;

        yield return new WaitForSeconds(_modifiedCooldown);

        canAttack = true;
    }

    #endregion

    #region Mobility

    [Header("Mobility")]
    [SerializeField] GameObject mobilitTelegraph;
    GameObject mobilityTelegraqphInstance;

    [SerializeField] GameObject mobilityStartPrefab;
    [SerializeField] GameObject mobilityEndPrefab;
    [SerializeField] GameObject mobilityHitEffect;

    [Header("Stats")]
    [SerializeField] int mobilityDamage;
    [SerializeField] float mobilityRange;

    [Header("Time")]
    [SerializeField] float mobilityCastTime;
    [SerializeField] float mobilityImpactTime;
    [SerializeField] float mobilityRecoveryTime;
    [SerializeField] float mobilityCoolDown;

    bool canDash = false;

    protected override void MobilityState()
    {
        modifiedCastTime = mobilityCastTime / CurrentAttackSpeed;
        modifiedImpactTime = mobilityImpactTime / CurrentAttackSpeed;
        modifiedRecoveryTime = mobilityRecoveryTime / CurrentAttackSpeed;

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

            if (mobilityTelegraqphInstance)
            {
                Destroy(mobilityTelegraqphInstance);
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

        if (canMobility && target != null && !hasAttacked)
        {
            canMobility = false;
            hasAttacked = true;

            // Set Cast Bar Color
            castBar.color = Color.yellow;

            // Calculate the direction from the enemy to the target
            directionToTarget = (target.position - transform.position).normalized;

            // Calculate the distance to the target
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // Check if the target is within mobilityRange
            if (distanceToTarget > mobilityRange)
            {
                // If the target is beyond mobilityRange, set the target position to the maximum range
                vectorToTarget = (Vector2)transform.position + directionToTarget * mobilityRange;
            }
            else
            {
                // If the target is within mobilityRange, set the target position to the target's position
                vectorToTarget = target.position;
            }

            // Play attack animation
            enemyAnimator.Play("Mobility Cast");
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            // Instantiate the telegraph
            mobilityTelegraqphInstance = Instantiate(mobilitTelegraph, vectorToTarget, Quaternion.identity);

            FillTelegraph fillTelegraph = mobilityTelegraqphInstance.GetComponent<FillTelegraph>();
            if (fillTelegraph != null)
            {
                fillTelegraph.FillSpeed = modifiedCastTime + modifiedImpactTime + modifiedRecoveryTime;
            }

            // Timers
            StartCoroutine(MobilityImpact());
            StartCoroutine(MobilityCoolDown());
        }

        if (canImpact)
        {
            canImpact = false;

            StartCoroutine(MobilityRecoveryTime());
        }
    }

    IEnumerator MobilityImpact()
    {
        UpdateCastBar(castBarTime, modifiedCastTime);

        yield return new WaitForSeconds(modifiedCastTime);

        if (!wasInterrupted)
        {
            canDash = true;

            Instantiate(mobilityStartPrefab, transform.position, transform.rotation);

            // Animate
            enemyAnimator.Play("Mobility Impact");

            // Set Cast Bar Color
            castBar.color = Color.green;

            // Delay
            StartCoroutine(MobilityImpactDelay());
        }
        else
        {
            wasInterrupted = false;
            hasAttacked = false;
        }
    }

    IEnumerator MobilityImpactDelay()
    {
        yield return new WaitForSeconds(modifiedImpactTime);

        canImpact = true;

        castBarTime = 0;
        StartCoroutine(EndCastBar());
    }

    IEnumerator MobilityRecoveryTime()
    {
        // Animate
        enemyAnimator.Play("Mobility Recovery");

        yield return new WaitForSeconds(modifiedRecoveryTime);

        canDash = false;

        if (target != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), false);
        }

        GameObject mobility = Instantiate(mobilityEndPrefab, vectorToTarget, Quaternion.identity);
        Instantiate(mobilityStartPrefab, transform.position, transform.rotation);

        // Cannot Hit Self with Attack
        Physics2D.IgnoreCollision(mobility.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

        DamageOnTrigger damageOnTrigger = mobility.GetComponent<DamageOnTrigger>();
        if (damageOnTrigger != null)
        {
            damageOnTrigger.AbilityDamage = mobilityDamage;
            damageOnTrigger.CharacterDamage = CurrentDamage;
            damageOnTrigger.HitEffect = mobilityHitEffect;
        }

        wasInterrupted = false;
        hasAttacked = false;

        enemyState = EnemyState.Idle;
    }

    IEnumerator MobilityCoolDown()
    {
        // Adjust cooldown time based on cooldown reduction
        float modifiedCooldown = mobilityCoolDown / CurrentCDR;

        yield return new WaitForSeconds(modifiedCooldown);

        canMobility = true;
    }

    #endregion

    #region Special

    [Header("Special")]
    [SerializeField] GameObject specialTelegraph;
    GameObject specialTelegraphInstance;

    [SerializeField] GameObject specialPrefab;
    [SerializeField] GameObject specialHitEffect;

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
            wasInterrupted = true;

            if (specialTelegraphInstance)
            {
                Destroy(specialTelegraphInstance);
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

        if (canSpecial && target != null && !hasAttacked)
        {
            canSpecial = false;
            hasAttacked = true;

            // Set Cast Bar Color
            castBar.color = Color.yellow;

            // Play attack animation
            enemyAnimator.Play("Special Cast");

            // Instantiate the telegraph
            specialTelegraphInstance = Instantiate(specialTelegraph, transform.position, Quaternion.identity, transform);

            FillTelegraph fillTelegraph = specialTelegraphInstance.GetComponent<FillTelegraph>();
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

        if (!wasInterrupted)
        {
            // Animate
            enemyAnimator.Play("Special Impact");

            // Set Cast Bar Color
            castBar.color = Color.green;

            // Delay
            StartCoroutine(SpecialImpactDelay());
        }
        else
        {
            wasInterrupted = false;
            hasAttacked = false;
        }
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

        float _modifiedRecoveryTime = specialRecoveryTime / CurrentAttackSpeed;

        yield return new WaitForSeconds(_modifiedRecoveryTime);

        wasInterrupted = false;
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
