using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    [Header("Attack")]
    [SerializeField] GameObject attackPrefab;
    [SerializeField] GameObject attackHitEffect;
    public int attackDamage;
    public float attackCastTime;
    public float attackRange;
    public float attackCoolDown;

    [Header("Mobility")]
    [SerializeField] GameObject mobilityPrefab;
    [SerializeField] GameObject mobilityHitEffect;
    public int mobilityDamage;
    public float durationOfMobility;
    public float mobilityCastTime;
    public float mobilityRange;
    public float mobilityCoolDown;

    [Header("Slow")]
    public float mobilitySlowAmount;
    public float mobilitySlowDuration;

    [Header("Special")]
    [SerializeField] GameObject specialPrefab;
    [SerializeField] GameObject specialHitEffect;
    public int specialDamage;
    public float durationOfSpecial;
    public float specialCastTime;
    public float specialRange;
    public float specialCoolDown;
    public float durationOfShell;

    [Header("KnockBack")]
    [SerializeField] float knockBackForce;
    [SerializeField] float knockBackDuration;

    float castTime = 0;
    bool canImpact = true;
    bool canRecovery = false;
    Vector2 directionToTarget;
    Vector2 dashDirection;

    //Telegraph
    [SerializeField] GameObject specialTelegraph;
    [SerializeField] GameObject shmackTelegraph;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (enemyState == EnemyState.Mobility)
        {
            if (castTime > mobilityCastTime)
            {
                if (target != null)
                {
                    // Disable collision between enemy and player
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), true);

                    // Use the stored dash direction for the enemy's velocity
                    enemyRB.velocity = dashDirection * mobilityRange;
                }
            }
        }
    }

    protected override void AttackState()
    {
        castTime += Time.deltaTime;
        UpdateCastBar(castTime, attackCastTime);

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
            Instantiate(shmackTelegraph, transform.position, Quaternion.Euler(0f, 0f, angle), transform);

            // Set animator parameters based on the direction
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            // Start Cooldown
            StartCoroutine(AttackCoolDown());
        }

        if (castTime > attackCastTime)
        {
            if (canImpact)
            {
                canImpact = false;

                castBar.color = Color.green;
                enemyAnimator.Play("Attack Impact");

                // Calculate the position for the attackPrefab
                Vector2 attackPosition = (Vector2)transform.position + directionToTarget * attackRange;

                GameObject attack = Instantiate(attackPrefab, attackPosition, Quaternion.identity);

                DamageOnTrigger damageOnTrigger = attack.GetComponent<DamageOnTrigger>();
                if (damageOnTrigger != null)
                {
                    damageOnTrigger.AbilityDamage = attackDamage;
                    damageOnTrigger.CharacterDamage = CurrentDamage;
                    damageOnTrigger.HitEffect = attackHitEffect;
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
        UpdateCastBar(0, attackCastTime);

        // State Transition
        enemyState = EnemyState.Idle;
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(attackCoolDown);

        canAttack = true;
    }

    protected override void MobilityState()
    {
        castTime += Time.deltaTime;
        UpdateCastBar(castTime, mobilityCastTime);

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

        if (canMobility)
        {
            canMobility = false;

            castBar.color = Color.yellow;

            // Play attack animation
            enemyAnimator.Play("Chase");

            // Immovable
            buffs.Immovable(durationOfMobility);

            StartCoroutine(DurationOfMobility());
            StartCoroutine(MobilityCoolDown());
        }

        if (castTime > mobilityCastTime)
        {
            if (canImpact && target != null)
            {
                canImpact = false;

                castBar.color = Color.green;

                // Calculate the direction from the enemy to the target
                directionToTarget = (target.position - transform.position).normalized;

                // Calculate the rotation towards the target
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Store the dash direction and angle
                dashDirection = directionToTarget;

                // Set animator parameters based on the direction
                enemyAnimator.SetFloat("Horizontal", dashDirection.x);
                enemyAnimator.SetFloat("Vertical", dashDirection.y);

                // Instantiate
                GameObject trail = Instantiate(mobilityPrefab, transform.position, rotation);

                SlowOnTrigger slowOnTrigger = trail.GetComponent<SlowOnTrigger>();
                if (slowOnTrigger != null)
                {
                    slowOnTrigger.SlowAmount = mobilitySlowAmount;
                    slowOnTrigger.SlowDuration = mobilitySlowDuration;
                }

                Destroy(trail, 3f);
            }
        }
    }

    IEnumerator DurationOfMobility()
    {
        yield return new WaitForSeconds(durationOfMobility);

        canImpact = true;

        castBar.color = Color.yellow;
        castTime = 0;
        UpdateCastBar(0, mobilityCastTime);

        if (target != null)
        {
            // Turn Back on collisions between enemy and player
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), false);
        }

        enemyState = EnemyState.Idle;
    }

    IEnumerator MobilityCoolDown()
    {
        yield return new WaitForSeconds(mobilityCoolDown);

        canMobility = true;
    }

    protected override void SpecialState()
    {
        castTime += Time.deltaTime;
        UpdateCastBar(castTime, mobilityCastTime);

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

        if (canSpecial && target != null)
        {
            canSpecial = false;

            castBar.color = Color.yellow;

            enemyAnimator.Play("Special Cast");

            // Calculate the direction from the enemy to the target
            directionToTarget = (target.position - transform.position).normalized;

            // Calculate the angle in degrees from the direction
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            // Instantiate the telegraph object at the enemy position with the appropriate rotation
            Instantiate(specialTelegraph, transform.position, Quaternion.Euler(0f, 0f, angle), transform);

            // Set animator parameters based on the direction
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            //StartCoroutine(SpecialCastTime());
            //StartCoroutine (DurationOfSpecial());
            StartCoroutine(SpecialCoolDown());
        }

        if (castTime > specialCastTime)
        {
            if (canImpact && target != null)
            {
                canImpact = false;

                castBar.color = Color.green;

                enemyAnimator.Play("Special Impact");

                // Set animator parameters based on the direction
                enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
                enemyAnimator.SetFloat("Vertical", directionToTarget.y);

                GameObject shell = Instantiate(specialPrefab, transform.position, Quaternion.identity);

                Rigidbody2D shellRB = shell.GetComponent<Rigidbody2D>();

                shellRB.AddForce(directionToTarget * specialRange, ForceMode2D.Impulse);

                Destroy(shell, durationOfShell);

                DamageOnTrigger damageOnTrigger = shell.GetComponent<DamageOnTrigger>();
                if (damageOnTrigger != null)
                {
                    damageOnTrigger.AbilityDamage = specialDamage;
                    damageOnTrigger.CharacterDamage = CurrentDamage;
                    damageOnTrigger.HitEffect = specialHitEffect;

                    damageOnTrigger.DestroyAfterDamage = true;
                }

                KnockbackOnTrigger knockbackOnTrigger = shell.GetComponent<KnockbackOnTrigger>();
                if (knockbackOnTrigger != null)
                {
                    knockbackOnTrigger.KnockBackForce = knockBackForce;
                    knockbackOnTrigger.KnockBackDuration = knockBackDuration;
                    knockbackOnTrigger.KnockBackDirection = directionToTarget;
                }
            }
        }

        if (canRecovery)
        {
            canRecovery = false;

            enemyAnimator.Play("Special Recovery");
        }
    }

    IEnumerator SpecialCoolDown()
    {
        yield return new WaitForSeconds(specialCoolDown);

        canSpecial = true;
    }
}
