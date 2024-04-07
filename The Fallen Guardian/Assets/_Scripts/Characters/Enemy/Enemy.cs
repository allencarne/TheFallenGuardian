using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float moveSpeed;
    public float maxMoveSpeed;
    public int damage;
    public float expToGive;

    [Header("Radius")]
    public float wanderRadius;
    public float attackRadius;
    public float mobilityRadius;
    public float deAggroRadius;

    [Header("Components")]
    EnemyHealthBar healthBar;
    [SerializeField] protected Image patienceBar;
    [SerializeField] protected Image castBar;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject floatingText;
    protected CrowdControl crowdControl;
    protected Animator enemyAnimator;
    protected Rigidbody2D enemyRB;
    [SerializeField] Collider2D enemyCollider2D;

    // Idle
    protected float idleTime;
    protected Vector2 startingPosition;
    int attemptsCount;

    // Wander
    Vector2 newWanderPosition;

    // Chase
    protected Transform target;
    bool playerInRange;
    public float patience;
    float patienceTime;

    // Bools
    bool canSpawn = true;
    bool canWander = true;
    protected bool canAttack = true;
    protected bool canMobility = true;
    bool canReset = true;

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
        crowdControl = GetComponent<CrowdControl>();
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
        if (enemyState == EnemyState.Wander && !crowdControl.isImmobilized)
        {
            // Calculate the direction
            Vector2 moveDirection = (newWanderPosition - (Vector2)transform.position).normalized;

            // Calculate the desired velocity to reach the new wander position
            Vector2 desiredVelocity = moveDirection * moveSpeed;

            // Apply the desired velocity to the rigidbody
            enemyRB.velocity = desiredVelocity;

            enemyAnimator.SetFloat("Horizontal", moveDirection.x);
            enemyAnimator.SetFloat("Vertical", moveDirection.y);
        }

        if (enemyState == EnemyState.Chase && !crowdControl.isImmobilized)
        {
            if (target != null)
            {
                Vector2 moveDirection = (target.position - transform.position).normalized;

                Vector2 desiredVelocity = moveDirection * moveSpeed;

                enemyRB.velocity = desiredVelocity;

                enemyAnimator.SetFloat("Horizontal", moveDirection.x);
                enemyAnimator.SetFloat("Vertical", moveDirection.y);
            }
        }

        if (enemyState == EnemyState.Reset && !crowdControl.isImmobilized)
        {
            // Calculate the direction
            Vector2 moveDirection = (startingPosition - (Vector2)transform.position).normalized;

            // Calculate the desired velocity to reach the new wander position
            Vector2 desiredVelocity = moveDirection * moveSpeed;

            // Apply the desired velocity to the rigidbody
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
            attemptsCount = 0;
            idleTime = 0;
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
            StartCoroutine(CheckReachedDestination(newWanderPosition));
        }

        if (playerInRange)
        {
            attemptsCount = 0;
            idleTime = 0;
            enemyState = EnemyState.Chase;
        }
    }

    IEnumerator CheckReachedDestination(Vector2 newPos)
    {
        while (Vector2.Distance(transform.position, newPos) > 0.1f)
        {
            yield return null;
        }

        // If enemy position is near the newWanderPosition - Set State to Idle
        enemyState = EnemyState.Idle;

        canWander = true;
        canReset = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startingPosition, wanderRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, mobilityRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startingPosition, deAggroRadius);
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

        if (target != null)
        {
            // Calculate the distance between the enemy and the target
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // Transition to Attack
            if (distanceToTarget <= attackRadius && !crowdControl.isDisarmed)
            {
                if (canAttack)
                {
                    enemyState = EnemyState.Attack;
                }
            }

            // Transition to Mobility
            if (distanceToTarget <= mobilityRadius && !crowdControl.isDisarmed)
            {
                if (canMobility)
                {
                    enemyState = EnemyState.Mobility;
                }
            }

            UpdatePatienceBar();

            // Calculate the distance between the starting Position and the target
            float distanceToStartingPosition = Vector2.Distance(startingPosition, target.position);

            // Check if the target is outside the deAggro radius
            if (distanceToStartingPosition >= deAggroRadius)
            {
                patienceTime += Time.deltaTime;

                if (patienceTime >= patience)
                {
                    patienceTime = 0;
                    playerInRange = false;
                    enemyState = EnemyState.Reset;
                }
            }
            else
            {
                patienceTime = 0;
            }
        }
        else
        {
            patienceTime = 0;
            playerInRange = false;
            enemyState = EnemyState.Reset;
        }
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
        UpdatePatienceBar();

        if (canReset)
        {
            canReset = false;

            enemyAnimator.Play("Wander");

            attemptsCount = 0;

            // Check if the enemy has reached the destination
            StartCoroutine(CheckReachedDestination(startingPosition));
        }
    }

    protected virtual void HurtState()
    {

    }

    protected virtual void DeathState()
    {
        enemyAnimator.Play("Death");
        enemyCollider2D.enabled = false;
        healthBar.enabled = false;

        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(.8f);

        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        idleTime = 0;

        StartCoroutine(FlashEffect(Color.red));

        healthBar.lerpTimer = 0f;

        ShowFloatingText(damage, Color.red);

        if (health <= 0)
        {
            enemyState = EnemyState.Death;
        }
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

    bool Immobilized = false;

    public void Immobilize()
    {
        if (Immobilized)
        {
            enemyRB.velocity = Vector2.zero;

            Immobilized = true;
        }
    }

    private void UpdatePatienceBar()
    {
        if (patienceBar != null)
        {
            // Calculate the fill amount
            float fillAmount = Mathf.Clamp01(patienceTime / patience);

            // Update the patience bar fill amount
            patienceBar.fillAmount = fillAmount;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            playerInRange = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;

            if (other.GetComponent<Player>().isPlayerOutOfHealth)
            {
                target = null;
                playerInRange = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //target = null;
            //playerInRange = false;
        }
    }

    public void UpdateCastBar(float fillAmount)
    {
        if (castBar != null)
        {
            castBar.fillAmount = fillAmount; // Directly set the fillAmount
        }
    }
}
