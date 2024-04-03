using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IKnockbackable
{
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float moveSpeed;
    public float maxMoveSpeed;
    public float damage;
    public float expToGive;

    [Header("Radius")]
    public float wanderRadius;
    public float attackRadius;

    [Header("Components")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject floatingText;
    EnemyHealthBar healthBar;
    protected Animator enemyAnimator;
    protected Rigidbody2D enemyRB;

    // Idle
    protected float idleTime;
    protected Vector2 startingPosition;
    int attemptsCount;

    // Wander
    Vector2 newWanderPosition;

    // Chase
    Transform target;
    bool playerInRange;

    // Attack
    public float durationOfAttack;

    // Bools
    bool canSpawn = true;
    bool canWander = true;
    bool canAttack = true;

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

    protected virtual void FixedUpdate()
    {
        if (enemyState == EnemyState.Wander)
        {
            // Calculate the direction towards the newWanderPosition
            Vector2 moveDirection = (newWanderPosition - (Vector2)transform.position).normalized;

            // Calculate the desired velocity to reach the new wander position
            Vector2 desiredVelocity = moveDirection * moveSpeed;

            // Apply the desired velocity to the rigidbody
            enemyRB.velocity = desiredVelocity;

            enemyAnimator.SetFloat("Horizontal", moveDirection.x);
            enemyAnimator.SetFloat("Vertical", moveDirection.y);
        }

        if (enemyState == EnemyState.Chase)
        {
            Vector2 moveDirection = (target.position - transform.position).normalized;

            Vector2 desiredVelocity = moveDirection * moveSpeed;

            enemyRB.velocity = desiredVelocity;

            enemyAnimator.SetFloat("Horizontal", moveDirection.x);
            enemyAnimator.SetFloat("Vertical", moveDirection.y);
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
        enemyAnimator.Play("Idle");

        idleTime += Time.deltaTime;

        if (idleTime >= 5)
        {
            int maxAttempts = 3; // Maximum number of consecutive failed attempts
            int consecutiveFailures = Mathf.Min(attemptsCount, maxAttempts);

            // Calculate the probability of transitioning to the wander state based on the number of consecutive failures
            float wanderProbability = Mathf.Min(0.5f + 0.25f * consecutiveFailures, 1.0f);

            // Check if the enemy will transition to the wander state based on the calculated probability
            if (Random.value < wanderProbability)
            {
                idleTime = 0;
                
                enemyState = EnemyState.Wander;
            }

            // Reset the idle time and update the attempts count
            idleTime = 0;
            attemptsCount++;
        }

        if (playerInRange)
        {
            enemyState = EnemyState.Chase;
        }
    }

    protected virtual void WanderState()
    {
        if (canWander)
        {
            canWander = false;

            enemyAnimator.Play("Wander");

            attemptsCount = 0;

            // Generate a newWanderPosition
            newWanderPosition = GetRandomPointInCircle(startingPosition, wanderRadius);

            // Check if the enemy has reached the new wander position
            StartCoroutine(CheckReachedDestination());
        }
    }

    IEnumerator CheckReachedDestination()
    {
        while (Vector2.Distance(transform.position, newWanderPosition) > 0.1f)
        {
            yield return null;
        }

        // If enemy position is near the newWanderPosition - Set State to Idle
        enemyState = EnemyState.Idle;

        canWander = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startingPosition, wanderRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private Vector2 GetRandomPointInCircle(Vector2 center, float radius)
    {
        // Generate a random angle
        float angle = Random.Range(0f, Mathf.PI * 2f);
        // Generate a random point within the circle
        float randomRadius = Random.Range(0f, radius);
        // Calculate the position
        Vector2 randomPoint = center + new Vector2(Mathf.Cos(angle) * randomRadius, Mathf.Sin(angle) * randomRadius);
        return randomPoint;
    }

    protected virtual void ChaseState()
    {
        enemyAnimator.Play("Chase");

        // Calculate the distance between the enemy and the target
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Check if the target is inside the attack radius
        if (distanceToTarget <= attackRadius)
        {
            // Target is within attack radius, transition to attack state
            enemyState = EnemyState.Attack;
        }
    }

    protected virtual void AttackState()
    {
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

            StartCoroutine(DurationOfAttack());
        }
    }

    IEnumerator DurationOfAttack()
    {
        yield return new WaitForSeconds(durationOfAttack);

        canAttack = true;

        enemyState = EnemyState.Idle;
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

        StartCoroutine(FlashEffect(Color.red));

        healthBar.lerpTimer = 0f;

        ShowFloatingText(damage, Color.red);
    }

    public void Heal(float heal)
    {
        health += heal;

        idleTime = 0;

        StartCoroutine(FlashEffect(Color.green));

        healthBar.lerpTimer = 0f;

        ShowFloatingText(heal, Color.green);
    }

    private IEnumerator FlashEffect(Color color)
    {
        float flashDuration = 0.1f;

        spriteRenderer.color = color;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = color;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration / 2);

        // Reset to original color
        spriteRenderer.color = Color.white;
    }

    void ShowFloatingText(float amount, Color color)
    {
        Vector3 offset = new Vector3(0f, 1, 0);

        if (floatingText)
        {
            GameObject textPrefab = Instantiate(floatingText, transform.position + offset, Quaternion.identity);
            TextMeshPro textMesh = textPrefab.GetComponentInChildren<TextMeshPro>();
            textMesh.text = amount.ToString();
            textMesh.color = color; // Set the color of the text
            Destroy(textPrefab, 1);
        }
    }

    public void KnockBack(Rigidbody2D opponentRB, float knockBackAmount, float knockBackDuration, Vector2 knockBackDirection)
    {
        // Use the passed knockBackDirection for applying the knockback force
        opponentRB.velocity = knockBackDirection * knockBackAmount;

        // Start a coroutine to handle the knockback duration
        StartCoroutine(KnockBackDuration(opponentRB, knockBackDuration));
    }

    IEnumerator KnockBackDuration(Rigidbody2D opponentRB, float duration)
    {
        float elapsedTime = 0f;
        Vector2 initialVelocity = opponentRB.velocity;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Normalized time
            opponentRB.velocity = Vector2.Lerp(initialVelocity, Vector2.zero, t);
            yield return null; // Wait for the next frame
        }

        opponentRB.velocity = Vector2.zero; // Ensure the velocity is exactly zero at the end
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            playerInRange = false;
        }
    }
}
