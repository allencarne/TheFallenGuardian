using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Snail : Enemy
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

                // Use the stored dash direction for the enemy's velocity
                enemyRB.velocity = dashDirection * mobilityRange;
            }

        }
    }

    protected override void HandleInterrupt()
    {
        if (castBar.color == Color.yellow)
        {
            castBar.color = Color.white;

            if (basicTelegraphInstance)
            {
                Destroy(basicTelegraphInstance);
            }

            if (basicEffectInstance)
            {
                Destroy(basicEffectInstance);
            }
            
            if (mobilityTelegraphInstance)
            {
                Destroy(mobilityTelegraphInstance);
            }

            if (mobilityEffectInstance)
            {
                Destroy(mobilityEffectInstance);
            }
            
            if (specialTelegraphInstance)
            {
                Destroy(specialTelegraphInstance);
            }

            if (specialEffectInstance)
            {
                Destroy(specialEffectInstance);
            }
            
        }
    }

    #region Basic

    [Header("Telegraph")]
    [SerializeField] GameObject basicTelegraph;
    GameObject basicTelegraphInstance;

    [Header("Effect")]
    [SerializeField] GameObject basicEffect;
    GameObject basicEffectInstance;

    [Header("Hit Effect")]
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
        modifiedRecoveryTime = basicRecoveryTime / CurrentAttackSpeed;

        UpdateCastBar(castBarTime, modifiedCastTime);

        Interrupt();

        // Cast
        if (canBasic && target != null && !hasAttacked)
        {
            // Set Bools
            canBasic = false;
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

            // Set Fill Speed
            FillTelegraph _fillTelegraph = basicTelegraphInstance.GetComponent<FillTelegraph>();
            if (_fillTelegraph != null)
            {
                _fillTelegraph.FillSpeed = modifiedCastTime;
            }

            // Cool Down
            StartCoroutine(BasicCoolDown());
        }

        // Impact
        if (castBarTime >= modifiedCastTime)
        {
            if (castBar.color == Color.yellow)
            {
                // Set Cast Bar Color
                castBar.color = Color.green;

                // Reset Timer
                castBarTime = 0;

                // Animate
                enemyAnimator.Play("Basic Impact");

                // Spawn Effect
                basicEffectInstance = Instantiate(basicEffect, vectorToTarget, Quaternion.identity);

                // Deal Damage
                DamageOnTrigger _damageOnTrigger = basicEffectInstance.GetComponent<DamageOnTrigger>();
                if (_damageOnTrigger != null)
                {
                    _damageOnTrigger.AbilityDamage = basicDamage;
                    _damageOnTrigger.CharacterDamage = CurrentDamage;
                    _damageOnTrigger.HitEffect = basicHitEffect;
                }
            }
        }

        // Recovery
        if (castBar.color == Color.green)
        {
            impactTime += Time.deltaTime;

            if (impactTime >= basicImpactTime)
            {
                // Set Cast Bar Color
                castBar.color = Color.grey;

                // Reset Timer
                impactTime = 0f;

                // Animate
                enemyAnimator.Play("Basic Recovery");
            }
        }

        // End
        if (castBar.color == Color.grey)
        {
            recoveryTime += Time.deltaTime;

            if (recoveryTime >= modifiedRecoveryTime)
            {
                // Set Cast Bar Color
                castBar.color = Color.black;

                // Reset Timer
                recoveryTime = 0f;

                // Reset Cast Bar
                castBar.fillAmount = 0;

                // Set bool
                hasAttacked = false;

                // Set State
                enemyState = EnemyState.Idle;
            }
        }
    }

    IEnumerator BasicCoolDown()
    {
        // Adjust cooldown time based on cooldown reduction
        float _modifiedCooldown = basicCoolDown / CurrentCDR;

        yield return new WaitForSeconds(_modifiedCooldown);

        canBasic = true;
    }

    #endregion

    #region Mobility

    [Header("Telegraph")]
    [SerializeField] GameObject mobilityTelegraph;
    GameObject mobilityTelegraphInstance;

    [Header("Effect")]
    [SerializeField] GameObject mobilityEffect;
    GameObject mobilityEffectInstance;

    [Header("Hit Effect")]
    [SerializeField] GameObject mobilityHitEffect;

    [Header("Stats")]
    [SerializeField] int mobilityDamage;
    [SerializeField] float mobilityRange;

    [Header("Time")]
    [SerializeField] float mobilityCastTime;
    [SerializeField] float mobilityImpactTime;
    [SerializeField] float mobilityRecoveryTime;
    [SerializeField] float mobilityCoolDown;

    [Header("Slow")]
    [SerializeField] int slowStacks;
    [SerializeField] float slowDuration;

    Vector2 dashDirection;
    Quaternion rotation;
    bool canDash = false;

    protected override void MobilityState()
    {
        modifiedCastTime = mobilityCastTime / CurrentAttackSpeed;
        modifiedImpactTime = mobilityImpactTime / CurrentAttackSpeed;
        modifiedRecoveryTime = mobilityRecoveryTime / CurrentAttackSpeed;

        UpdateCastBar(castBarTime, modifiedCastTime);

        Interrupt();

        // Cast
        if (canMobility && target != null && !hasAttacked)
        {
            // Set Bools
            canMobility = false;
            hasAttacked = true;

            // Set Cast Bar Color
            castBar.color = Color.yellow;

            // Calculate the direction from the enemy to the target
            directionToTarget = (target.position - transform.position).normalized;

            // Calculate the rotation towards the target
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Store the dash direction and angle
            dashDirection = directionToTarget;

            // Play attack animation
            enemyAnimator.Play("Mobility Cast");
            enemyAnimator.SetFloat("Horizontal", dashDirection.x);
            enemyAnimator.SetFloat("Vertical", dashDirection.y);

            mobilityTelegraphInstance = Instantiate(mobilityTelegraph, transform.position, rotation);

            // Set Fill Speed
            FillTelegraph _fillTelegraph = mobilityTelegraphInstance.GetComponent<FillTelegraph>();
            if (_fillTelegraph != null)
            {
                _fillTelegraph.FillSpeed = modifiedCastTime;
            }

            // Cool Down
            StartCoroutine(MobilityCoolDown());
        }

        // Impact
        if (castBarTime >= modifiedCastTime)
        {
            if (castBar.color == Color.yellow)
            {
                // Set Cast Bar Color
                castBar.color = Color.green;

                // Reset Timer
                castBarTime = 0;

                // Animate
                enemyAnimator.Play("Mobility Impact");

                // Immovable
                immovable.Immovable(modifiedImpactTime + .2f);

                // Protection
                protection.Protection(2, 3);

                // Effect
                mobilityEffectInstance = Instantiate(mobilityEffect, transform.position, rotation);

                // Destroy
                Destroy(mobilityEffectInstance, 3f);

                // Slow
                SlowOnTrigger slowOnTrigger = mobilityEffectInstance.GetComponent<SlowOnTrigger>();
                if (slowOnTrigger != null)
                {
                    slowOnTrigger.Stacks = slowStacks;
                    slowOnTrigger.Duration = slowDuration;
                }

                // Bool for FixedUpdate
                canDash = true;
            }
        }

        // Recovery
        if (castBar.color == Color.green)
        {
            impactTime += Time.deltaTime;

            if (impactTime >= modifiedImpactTime)
            {
                // Set Cast Bar Color
                castBar.color = Color.grey;

                // Reset Timer
                impactTime = 0f;

                // Animate
                enemyAnimator.Play("Mobility Recovery");

                // Bool for FixedUpdate
                canDash = false;

                // Enemy can collide with Target
                if (target != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), false);
                }
            }
        }

        // End
        if (castBar.color == Color.grey)
        {
            recoveryTime += Time.deltaTime;

            if (recoveryTime >= modifiedRecoveryTime)
            {
                // Set Cast Bar Color
                castBar.color = Color.black;

                // Reset Timer
                recoveryTime = 0f;

                // Reset Cast Bar
                castBar.fillAmount = 0;

                // Set bool
                hasAttacked = false;

                // Set State
                enemyState = EnemyState.Idle;
            }
        }
    }

    IEnumerator MobilityCoolDown()
    {
        yield return new WaitForSeconds(mobilityCoolDown);

        canMobility = true;
    }

    #endregion

    #region Special

    [Header("Telegraph")]
    [SerializeField] GameObject specialTelegraph;
    GameObject specialTelegraphInstance;

    [Header("Effect")]
    [SerializeField] GameObject specialEffect;
    GameObject specialEffectInstance;

    [Header("Hit Effect")]
    [SerializeField] GameObject specialHitEffect;

    [Header("Stats")]
    [SerializeField] int specialDamage;

    [Header("Time")]
    [SerializeField] float specialCastTime;
    [SerializeField] float specialImpactTime;
    [SerializeField] float specialRecoveryTime;
    [SerializeField] float specialCoolDown;

    [SerializeField] float durationOfShell;
    [SerializeField] float shellSpeed;

    [Header("Knock Back")]
    [SerializeField] float knockBackForce;
    [SerializeField] float knockBackDuration;

    protected override void SpecialState()
    {
        modifiedCastTime = specialCastTime / CurrentAttackSpeed;
        modifiedRecoveryTime = specialRecoveryTime / CurrentAttackSpeed;

        UpdateCastBar(castBarTime, modifiedCastTime);

        Interrupt();

        // Cast
        if (canSpecial && target != null && !hasAttacked)
        {
            // Set Bools
            canSpecial = false;
            hasAttacked = true;

            // Set Cast Bar Color
            castBar.color = Color.yellow;

            // Calculate the direction from the enemy to the target
            directionToTarget = (target.position - transform.position).normalized;

            // Calculate the angle in degrees from the direction
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Play attack animation
            enemyAnimator.Play("Special Cast");
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            // Instantiate the telegraph
            specialTelegraphInstance = Instantiate(specialTelegraph, transform.position, Quaternion.Euler(0f, 0f, angle));

            FillTelegraph _fillTelegraph = specialTelegraphInstance.GetComponent<FillTelegraph>();
            if (_fillTelegraph != null)
            {
                _fillTelegraph.FillSpeed = modifiedCastTime;
            }

            // Cool Down
            StartCoroutine(SpecialCoolDown());
        }

        // Impact
        if (castBarTime >= modifiedCastTime)
        {
            if (castBar.color == Color.yellow)
            {
                // Set Cast Bar Color
                castBar.color = Color.green;

                // Reset Timer
                castBarTime = 0;

                // Animate
                enemyAnimator.Play("Special Impact");

                // Spawn Effect
                specialEffectInstance = Instantiate(specialEffect, transform.position, Quaternion.identity);

                // Get RB
                Rigidbody2D shellRB = specialEffectInstance.GetComponent<Rigidbody2D>();

                // Add Force
                shellRB.AddForce(directionToTarget * shellSpeed, ForceMode2D.Impulse);

                // Destroy
                Destroy(specialEffectInstance, durationOfShell);

                // Deal Damage
                DamageOnTrigger _damageOnTrigger = specialEffectInstance.GetComponent<DamageOnTrigger>();
                if (_damageOnTrigger != null)
                {
                    _damageOnTrigger.AbilityDamage = specialDamage;
                    _damageOnTrigger.CharacterDamage = CurrentDamage;
                    _damageOnTrigger.HitEffect = specialHitEffect;
                }

                KnockbackOnTrigger knockbackOnTrigger = specialEffectInstance.GetComponent<KnockbackOnTrigger>();
                if (knockbackOnTrigger != null)
                {
                    knockbackOnTrigger.KnockBackForce = knockBackForce;
                    knockbackOnTrigger.KnockBackDuration = knockBackDuration;
                    knockbackOnTrigger.KnockBackDirection = directionToTarget;
                }
            }
        }

        // Recovery
        if (castBar.color == Color.green)
        {
            impactTime += Time.deltaTime;

            if (impactTime >= specialImpactTime)
            {
                // Set Cast Bar Color
                castBar.color = Color.grey;

                // Reset Timer
                impactTime = 0f;

                // Animate
                enemyAnimator.Play("Special Recovery");
            }
        }

        // End
        if (castBar.color == Color.grey)
        {
            recoveryTime += Time.deltaTime;

            if (recoveryTime >= modifiedRecoveryTime)
            {
                // Set Cast Bar Color
                castBar.color = Color.black;

                // Reset Timer
                recoveryTime = 0f;

                // Reset Cast Bar
                castBar.fillAmount = 0;

                // Set bool
                hasAttacked = false;

                // Set State
                enemyState = EnemyState.Idle;
            }
        }
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
