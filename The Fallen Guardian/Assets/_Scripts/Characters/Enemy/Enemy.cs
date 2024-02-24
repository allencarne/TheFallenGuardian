using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable, IKnockbackable
{
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float moveSpeed;
    public float maxMoveSpeed;
    public float damage;
    public float expToGive;

    [SerializeField] SpriteRenderer spriteRenderer;
    float flashDuration = 0.1f;
    protected Animator enemyAnimator;
    protected Rigidbody2D enemyRB;
    protected Vector2 startingPosition;

    EnemyHealthBar healthBar;

    protected float idleTime;
    bool canSpawn = true;

    protected enum EnemyState 
    { 
        Spawn,
        Idle,
        Wander,
        Chase,
        Attack,
        Mobility,
        Special,
        Reset,
        Hurt,
        Death
    }

    protected EnemyState enemyState = EnemyState.Spawn;

    private void Awake()
    {
        healthBar = GetComponent<EnemyHealthBar>();
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Set Health
        health = maxHealth;

        // Set Starting Position
        startingPosition = transform.position;
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Spawn:
                SpawnState();
                break;
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.Wander:
                WanderState();
                break;
            case EnemyState.Chase:
                ChaseState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Mobility:
                MobilityState();
                break;
            case EnemyState.Special:
                SpecialState();
                break;
            case EnemyState.Reset:
                ResetState();
                break;
            case EnemyState.Hurt:
                HurtState();
                break;
            case EnemyState.Death:
                DeathState();
                break;
        }
    }

    protected virtual void SpawnState()
    {
        if (canSpawn)
        {
            canSpawn = false;

            enemyAnimator.Play("Spawn");

            StartCoroutine(SpawnDuration());
        }
    }

    IEnumerator SpawnDuration()
    {
        yield return new WaitForSeconds(.6f);

        enemyState = EnemyState.Idle;

        canSpawn = true;
    }

    protected virtual void IdleState()
    {

    }

    protected virtual void WanderState()
    {

    }

    protected virtual void ChaseState()
    {

    }

    protected virtual void AttackState()
    {

    }

    protected virtual void MobilityState()
    {

    }

    protected virtual void SpecialState()
    {

    }

    protected virtual void ResetState()
    {

    }

    protected virtual void HurtState()
    {

    }

    protected virtual void DeathState()
    {

    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        idleTime = 0;

        StartCoroutine(FlashOnDamage());

        Debug.Log("TakeDamage" + damage);

        // Healthbar Lerp
        healthBar.lerpTimer = 0f;
    }

    public void Heal(float heal)
    {
        health += heal;

        idleTime = 0;

        // Healthbar Lerp
        healthBar.lerpTimer = 0f;
    }

    private IEnumerator FlashOnDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration / 2);

        // Reset to original color
        spriteRenderer.color = Color.white;
    }

    public void KnockBack(Vector3 opponentPosition, Vector3 yourPosition, Rigidbody2D opponentRB, float knockBackAmount)
    {
        // Knock Back
        Vector2 direction = (opponentPosition - yourPosition).normalized;
        opponentRB.velocity = direction * knockBackAmount;

        StartCoroutine(KnockBackDuration(opponentRB));
    }

    IEnumerator KnockBackDuration(Rigidbody2D opponentRB)
    {
        yield return new WaitForSeconds(.3f);

        opponentRB.velocity = Vector2.zero;
    }
}
