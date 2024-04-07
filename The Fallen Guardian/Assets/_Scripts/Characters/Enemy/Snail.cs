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

    private Vector2 dashDirection;

    [Header("Special")]
    [SerializeField] GameObject specialPrefab;
    [SerializeField] GameObject specialHitEffect;
    public int specialDamage;
    public float durationOfSpecial;
    public float specialCastTime;
    public float specialRange;
    public float specialCoolDown;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (enemyState == EnemyState.Mobility)
        {
            // Disable collision between enemy and player
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), true);

            // Use the stored dash direction for the enemy's velocity
            enemyRB.velocity = dashDirection * mobilityRange;
        }
    }

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
            StartCoroutine(AttackCoolDown());
        }
    }

    IEnumerator CastTime(Vector2 directionToTarget)
    {
        yield return new WaitForSeconds(attackCastTime);

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

        if (enemyState == EnemyState.Attack)
        {
            enemyState = EnemyState.Idle;
        }
    }

    IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(attackCoolDown);

        canAttack = true;
    }

    protected override void MobilityState()
    {
        if (canMobility)
        {
            canMobility = false;

            enemyAnimator.Play("Chase");

            // Calculate the direction from the enemy to the target
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            // Set animator parameters based on the direction
            enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
            enemyAnimator.SetFloat("Vertical", directionToTarget.y);

            // Calculate the rotation towards the target
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Store the dash direction and angle
            dashDirection = directionToTarget;

            GameObject trail = Instantiate(mobilityPrefab, transform.position, rotation);

            Destroy(trail, 3f);

            StartCoroutine(DurationOfMobility());
            StartCoroutine(MobilityCoolDown());
        }
    }

    IEnumerator DurationOfMobility()
    {
        yield return new WaitForSeconds(durationOfMobility);

        enemyState = EnemyState.Idle;

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), target.GetComponent<Collider2D>(), false);
    }

    IEnumerator MobilityCoolDown()
    {
        yield return new WaitForSeconds(mobilityCoolDown);

        canMobility = true;
    }

    protected override void SpecialState()
    {
        if (canSpecial)
        {
            canSpecial = false;

            enemyAnimator.Play("Special");

            StartCoroutine(SpecialCastTime());
            StartCoroutine (DurationOfSpecial());
            StartCoroutine(SpecialCoolDown());
        }
    }

    IEnumerator SpecialCastTime()
    {
        yield return new WaitForSeconds(specialCastTime);

        // Calculate the direction from the enemy to the target
        Vector2 directionToTarget = (target.position - transform.position).normalized;

        // Set animator parameters based on the direction
        enemyAnimator.SetFloat("Horizontal", directionToTarget.x);
        enemyAnimator.SetFloat("Vertical", directionToTarget.y);

        // Store the dash direction and angle
        dashDirection = directionToTarget;

        GameObject shell = Instantiate(specialPrefab, transform.position, Quaternion.identity);

        Rigidbody2D shellRB = shell.GetComponent<Rigidbody2D>();

        shellRB.AddForce(directionToTarget * specialRange, ForceMode2D.Impulse);

        Destroy(shell, 1.5f);

        // Ignore collision between the attack and the caster
        Physics2D.IgnoreCollision(shell.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

        DamageOnTrigger damageOnTrigger = shell.GetComponent<DamageOnTrigger>();
        if (damageOnTrigger != null)
        {
            damageOnTrigger.abilityDamage = specialDamage;
            damageOnTrigger.characterDamage = damage;
            damageOnTrigger.hitEffect = specialHitEffect;
        }
    }

    IEnumerator DurationOfSpecial()
    {
        yield return new WaitForSeconds(durationOfSpecial);

        enemyState = EnemyState.Idle;
    }

    IEnumerator SpecialCoolDown()
    {
        yield return new WaitForSeconds(specialCoolDown);

        canSpecial = true;
    }
}
