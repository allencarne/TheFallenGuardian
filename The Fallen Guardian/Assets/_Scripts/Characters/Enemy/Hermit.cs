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

            if (mobilityEndEffectInstance)
            {
                Destroy(mobilityEndEffectInstance);
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
    [SerializeField] GameObject mobilityStartEffect;
    [SerializeField] GameObject mobilityEndEffect;
    GameObject mobilityEndEffectInstance;

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

            // Set Fill Speed
            FillTelegraph _fillTelegraph = mobilityTelegraphInstance.GetComponent<FillTelegraph>();
            if (_fillTelegraph != null)
            {
                _fillTelegraph.FillSpeed = modifiedCastTime + modifiedImpactTime + modifiedRecoveryTime;
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

                // Dust Effect
                Instantiate(mobilityStartEffect, transform.position, transform.rotation);

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

                // Bool for FixedUpdate
                canDash = false;

                // Enemy can collide with Target
                if (target != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), false);
                }

                // Damage Effect
                mobilityEndEffectInstance = Instantiate(mobilityEndEffect, vectorToTarget, Quaternion.identity);

                // Damage
                DamageOnTrigger _damageOnTrigger = mobilityEndEffectInstance.GetComponent<DamageOnTrigger>();
                if (_damageOnTrigger != null)
                {
                    _damageOnTrigger.AbilityDamage = mobilityDamage;
                    _damageOnTrigger.CharacterDamage = CurrentDamage;
                    _damageOnTrigger.HitEffect = mobilityHitEffect;
                }

                // Slow
                SlowOnTrigger slowOnTrigger = mobilityEndEffectInstance.GetComponent<SlowOnTrigger>();
                if (slowOnTrigger != null)
                {
                    slowOnTrigger.Stacks = slowStacks;
                    slowOnTrigger.Duration = slowDuration;
                }

                // Set State
                enemyState = EnemyState.Idle;
            }
        }
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

            // Play attack animation
            enemyAnimator.Play("Special Cast");

            // Instantiate the telegraph
            specialTelegraphInstance = Instantiate(specialTelegraph, transform.position, Quaternion.identity, transform);

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

                // Protection
                protection.Protection(4,3);

                // Spawn Effect
                specialEffectInstance = Instantiate(specialEffect, transform.position, Quaternion.identity, transform);

                // Deal Damage
                DamageOnTrigger _damageOnTrigger = specialEffectInstance.GetComponent<DamageOnTrigger>();
                if (_damageOnTrigger != null)
                {
                    _damageOnTrigger.AbilityDamage = specialDamage;
                    _damageOnTrigger.CharacterDamage = CurrentDamage;
                    _damageOnTrigger.HitEffect = specialHitEffect;
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
