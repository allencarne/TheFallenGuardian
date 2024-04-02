using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    protected Animator enemyAnimator;
    protected Rigidbody2D enemyRB;
    protected Vector2 startingPosition;
    public float wanderRadius = 5f;
    Vector2 newWanderPosition;

    EnemyHealthBar healthBar;
    [SerializeField] GameObject floatingText;

    protected float idleTime;
    bool canSpawn = true;
    bool canWander = true;

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

        idleTime += 1 * Time.deltaTime;

        if (idleTime >= 5)
        {
            int choice = Random.Range(0, 2);
            switch (choice)
            {
                case 0:
                    enemyState = EnemyState.Wander;
                    idleTime = 0;
                    break;
                case 1:
                    idleTime = 0;
                    break;
            }
        }
    }

    protected virtual void WanderState()
    {
        if (canWander)
        {
            canWander = false;

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
        // Draw a wire sphere gizmo to visualize the wander radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startingPosition, wanderRadius);
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
}
