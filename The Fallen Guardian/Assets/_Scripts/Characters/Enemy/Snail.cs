using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    private float dashAngle;

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
        float remainingCastTime = attackCastTime;
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
            float progress = 1 - (remainingCastTime / attackCastTime);
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
            dashAngle = angle;

            var trail = Instantiate(mobilityPrefab, transform.position, rotation);

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
}
