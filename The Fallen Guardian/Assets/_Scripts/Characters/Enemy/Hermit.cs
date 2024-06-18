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

    [SerializeField] GameObject basicEffect;
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

        UpdateCastBar(castBarTime, modifiedCastTime);

        if (crowdControl.IsInterrupted)
        {
            crowdControl.IsInterrupted = false;
            wasInterrupted = true;

            if (wasInterrupted)
            {
                if (castBar.color != Color.green)
                {
                    if (basicTelegraphInstance)
                    {
                        Destroy(basicTelegraphInstance);
                    }

                    // State Transition
                    enemyState = EnemyState.Idle;

                    StartCoroutine(InterruptCastBar());
                    return;
                }
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

            GameObject _basic = Instantiate(basicEffect, vectorToTarget, Quaternion.identity);

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

            enemyState = EnemyState.Idle;

            StartCoroutine(ResetCastBar());
        }
    }

    IEnumerator BasicImpact()
    {
        yield return new WaitForSeconds(modifiedImpactTime);

        // Animate
        enemyAnimator.Play("Basic Recovery");

        StartCoroutine(EndCastBar());
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
    [SerializeField] GameObject mobilityTelegraph;
    GameObject mobilityTelegraphInstance;

    [SerializeField] GameObject mobilityStartEffect;
    [SerializeField] GameObject mobilityEndEffect;
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

        UpdateCastBar(castBarTime, modifiedCastTime);

        if (crowdControl.IsInterrupted)
        {
            crowdControl.IsInterrupted = false;
            wasInterrupted = true;

            if (wasInterrupted)
            {
                if (castBar.color != Color.green)
                {
                    if (mobilityTelegraphInstance)
                    {
                        Destroy(mobilityTelegraphInstance);
                    }

                    // State Transition
                    enemyState = EnemyState.Idle;

                    StartCoroutine(InterruptCastBar());
                    return;
                }
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
            float _distanceToTarget = Vector2.Distance(transform.position, target.position);

            // Check if the target is within mobilityRange
            if (_distanceToTarget > mobilityRange)
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
            mobilityTelegraphInstance = Instantiate(mobilityTelegraph, vectorToTarget, Quaternion.identity);

            FillTelegraph _fillTelegraph = mobilityTelegraphInstance.GetComponent<FillTelegraph>();
            if (_fillTelegraph != null)
            {
                _fillTelegraph.FillSpeed = modifiedCastTime + modifiedImpactTime + modifiedRecoveryTime;
            }

            // Timers
            StartCoroutine(MobilityCast());
            StartCoroutine(MobilityCoolDown());
        }
    }

    IEnumerator MobilityCast()
    {
        yield return new WaitForSeconds(modifiedCastTime);

        if (!wasInterrupted)
        {
            // Animate
            enemyAnimator.Play("Mobility Impact");

            // Set Cast Bar Color
            castBar.color = Color.green;

            // Dust Effect
            Instantiate(mobilityStartEffect, transform.position, transform.rotation);

            // Bool for FixedUpdate
            canDash = true;

            // Delay
            StartCoroutine(MobilityImpact());
        }
        else
        {
            wasInterrupted = false;
            hasAttacked = false;

            enemyState = EnemyState.Idle;

            StartCoroutine(ResetCastBar());
        }
    }

    IEnumerator MobilityImpact()
    {
        yield return new WaitForSeconds(modifiedImpactTime);

        // Animate
        enemyAnimator.Play("Mobility Recovery");

        StartCoroutine(EndCastBar());
        StartCoroutine(MobilityRecovery());
    }

    IEnumerator MobilityRecovery()
    {
        yield return new WaitForSeconds(modifiedRecoveryTime);

        // Bool for FixedUpdate
        canDash = false;

        // Enemy can collide with Target
        if (target != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), false);
        }

        // Damage Effect
        GameObject _mobility = Instantiate(mobilityEndEffect, vectorToTarget, Quaternion.identity);
        
        // Dust Effect
        Instantiate(mobilityStartEffect, transform.position, transform.rotation);

        // Cannot Hit Self with Attack
        Physics2D.IgnoreCollision(_mobility.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

        DamageOnTrigger _damageOnTrigger = _mobility.GetComponent<DamageOnTrigger>();
        if (_damageOnTrigger != null)
        {
            _damageOnTrigger.AbilityDamage = mobilityDamage;
            _damageOnTrigger.CharacterDamage = CurrentDamage;
            _damageOnTrigger.HitEffect = mobilityHitEffect;
        }

        wasInterrupted = false;
        hasAttacked = false;

        enemyState = EnemyState.Idle;
    }

    IEnumerator MobilityCoolDown()
    {
        // Adjust cooldown time based on cooldown reduction
        float _modifiedCooldown = mobilityCoolDown / CurrentCDR;

        yield return new WaitForSeconds(_modifiedCooldown);

        canMobility = true;
    }

    #endregion

    #region Special

    [Header("Special")]
    [SerializeField] GameObject specialTelegraph;
    GameObject specialTelegraphInstance;

    [SerializeField] GameObject specialEffect;
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
        modifiedRecoveryTime = specialRecoveryTime / CurrentAttackSpeed;

        UpdateCastBar(castBarTime, modifiedCastTime);

        if (crowdControl.IsInterrupted)
        {
            crowdControl.IsInterrupted = false;
            wasInterrupted = true;

            if (wasInterrupted)
            {
                if (castBar.color != Color.green)
                {
                    if (specialTelegraphInstance)
                    {
                        Destroy(specialTelegraphInstance);
                    }

                    // State Transition
                    enemyState = EnemyState.Idle;

                    StartCoroutine(InterruptCastBar());
                    return;
                }
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

            FillTelegraph _fillTelegraph = specialTelegraphInstance.GetComponent<FillTelegraph>();
            if (_fillTelegraph != null)
            {
                _fillTelegraph.FillSpeed = modifiedCastTime;
            }

            // Timers
            StartCoroutine(SpecialCast());
            StartCoroutine(SpecialCoolDown());
        }
    }

    IEnumerator SpecialCast()
    {
        yield return new WaitForSeconds(modifiedCastTime);

        if (!wasInterrupted)
        {
            // Animate
            enemyAnimator.Play("Special Impact");

            // Set Cast Bar Color
            castBar.color = Color.green;

            StartCoroutine(AttackRate());

            // Delay
            StartCoroutine(SpecialImpact());
        }
        else
        {
            wasInterrupted = false;
            hasAttacked = false;

            enemyState = EnemyState.Idle;

            StartCoroutine(ResetCastBar());
        }
    }

    IEnumerator AttackRate()
    {
        Attack();
        yield return new WaitForSeconds(specialAttackRate);
        Attack();
        yield return new WaitForSeconds(specialAttackRate);
        Attack();
        yield return new WaitForSeconds(specialAttackRate);
        Attack();
    }

    void Attack()
    {
        GameObject _special = Instantiate(specialEffect, transform.position, Quaternion.identity, transform);

        // Cannot Hit Self with Attack
        Physics2D.IgnoreCollision(_special.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

        DamageOnTrigger _damageOnTrigger = _special.GetComponent<DamageOnTrigger>();
        if (_damageOnTrigger != null)
        {
            _damageOnTrigger.AbilityDamage = specialDamage;
            _damageOnTrigger.CharacterDamage = CurrentDamage;
            _damageOnTrigger.HitEffect = specialHitEffect;
        }
    }

    IEnumerator SpecialImpact()
    {
        yield return new WaitForSeconds(specialImpactTime);

        // Animate
        enemyAnimator.Play("Special Recovery");

        StartCoroutine(EndCastBar());
        StartCoroutine(SpecialRecovery());
    }

    IEnumerator SpecialRecovery()
    {
        yield return new WaitForSeconds(modifiedRecoveryTime);

        wasInterrupted = false;
        hasAttacked = false;

        enemyState = EnemyState.Idle;
    }

    IEnumerator SpecialCoolDown()
    {
        // Adjust cooldown time based on cooldown reduction
        float _modifiedCooldown = specialCoolDown / CurrentCDR;

        yield return new WaitForSeconds(_modifiedCooldown);

        canSpecial = true;
    }

    #endregion
}
