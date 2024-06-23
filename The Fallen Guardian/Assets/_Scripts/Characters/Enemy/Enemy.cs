using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float Health;
    public float MaxHealth;
    [Header("Speed")]
    public float BaseSpeed; 
    public float CurrentSpeed;
    [Header("Damage")]
    public int BaseDamage;
    public int CurrentDamage;
    [Header("AttackSpeed")]
    public float BaseAttackSpeed;
    public float CurrentAttackSpeed;
    [Header("CDR")]
    public float BaseCDR;
    public float CurrentCDR;
    [Header("Armor")]
    public float BaseArmor;
    public float CurrentArmor;

    [Header("Exp")]
    public float expToGive;

    [Header("Radius")]
    public float wanderRadius;
    public float attackRadius;
    public float mobilityRadius;
    public float specialRadius;
    public float deAggroRadius;

    [Header("Components")]
    [HideInInspector] public EnemySpawner EnemySpawner;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D enemyCollider2D;
    [SerializeField] GameObject shadow;
    protected Rigidbody2D enemyRB;
    protected Animator enemyAnimator;
    EnemyDrop enemyDrop;

    [Header("Status Effects")]
    protected CrowdControl crowdControl;
    Buff_Regeneration regeneration;
    protected Buff_Protection protection;
    protected Buff_Immovable immovable;

    [Header("Bars")]
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] protected Image patienceBar;
    [SerializeField] protected Image castBar;

    [Header("Idle")]
    protected float idleTime;
    protected Vector2 startingPosition;
    int attemptsCount;

    [Header("Wander")]
    Vector2 newWanderPosition;

    [Header("Chase")]
    protected Transform target;
    bool playerInRange;
    public float patience;
    float patienceTime;

    // Remove?
    bool Immobilized = false;

    [Header("Bools")]
    protected bool canBasic = true;
    protected bool canMobility = true;
    protected bool canSpecial = true;
    bool canSpawn = true;
    bool canWander = true;
    bool canReset = true;
    bool canDeath = true;
    bool isRegenerating = false;

    [Header("Attack")]
    protected bool hasAttacked = false;
    protected Vector2 directionToTarget;
    protected Vector2 vectorToTarget;

    [Header("Timers")]
    protected float modifiedCastTime;
    protected float modifiedImpactTime;
    protected float modifiedRecoveryTime;
    protected float castBarTime = 0;
    protected float impactTime = 0f;
    protected float recoveryTime = 0f;

    [Header("Events")]
    public UnityEvent OnHealthChanged;
    public UnityEvent OnDeath;

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
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyDrop = GetComponent<EnemyDrop>();

        crowdControl = GetComponent<CrowdControl>();
        regeneration = GetComponent<Buff_Regeneration>();
        protection = GetComponent<Buff_Protection>();
        immovable = GetComponent<Buff_Immovable>();
    }

    private void Start()
    {
        // Set Health
        Health = MaxHealth;

        // Set Starting Position
        startingPosition = transform.position;

        // Set Speed
        CurrentSpeed = BaseSpeed;

        // Set Damage
        CurrentDamage = BaseDamage;

        // Set Attack Speed
        CurrentAttackSpeed = BaseAttackSpeed;

        // Set CDR
        CurrentCDR = BaseCDR;

        // Set Armor
        CurrentArmor = BaseArmor;
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

        // Temporary Solution - Could be bugs if the Delay isn't long enough
        if (target != null)
        {
            Player player = target.GetComponent<Player>();
            if (player)
            {
                if (player.Stats.Health <= 0)
                {
                    StartCoroutine(DropTarget());
                }
            }
        }
    }

    IEnumerator DropTarget()
    {
        target = null;
        playerInRange = false;

        yield return new WaitForSeconds(1.5f);

        enemyState = EnemyState.Reset;
    }

    protected virtual void FixedUpdate()
    {
        if (enemyState == EnemyState.Wander && !crowdControl.IsImmobilized)
        {
            // Calculate the direction
            Vector2 moveDirection = (newWanderPosition - (Vector2)transform.position).normalized;

            // Calculate the desired velocity to reach the new wander position
            Vector2 desiredVelocity = moveDirection * CurrentSpeed;

            // Apply the desired velocity to the rigidbody
            enemyRB.velocity = desiredVelocity;

            enemyAnimator.SetFloat("Horizontal", moveDirection.x);
            enemyAnimator.SetFloat("Vertical", moveDirection.y);
        }

        if (enemyState == EnemyState.Chase && !crowdControl.IsImmobilized)
        {
            if (target != null)
            {
                Vector2 moveDirection = (target.position - transform.position).normalized;

                Vector2 desiredVelocity = moveDirection * CurrentSpeed;

                enemyRB.velocity = desiredVelocity;

                enemyAnimator.SetFloat("Horizontal", moveDirection.x);
                enemyAnimator.SetFloat("Vertical", moveDirection.y);
            }
        }

        if (enemyState == EnemyState.Reset && !crowdControl.IsImmobilized)
        {
            // Calculate the direction
            Vector2 moveDirection = (startingPosition - (Vector2)transform.position).normalized;

            // Calculate the desired velocity to reach the new wander position
            Vector2 desiredVelocity = moveDirection * CurrentSpeed;

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

        enemyCollider2D.enabled = true;

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
            if (distanceToTarget <= attackRadius && !crowdControl.IsDisarmed)
            {
                if (canBasic)
                {
                    enemyState = EnemyState.Attack;
                }
            }

            // Transition to Mobility
            if (distanceToTarget <= mobilityRadius)
            {
                if (canMobility)
                {
                    enemyState = EnemyState.Mobility;
                }
            }

            // Transition to Special
            if (distanceToTarget <= specialRadius)
            {
                if (canSpecial)
                {
                    enemyState = EnemyState.Special;
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

        if (Health < MaxHealth && !isRegenerating)
        {
            isRegenerating = true;

            // Calculate Missing Health
            float missingHealth = MaxHealth - Health;

            // Start CoRoutine
            StartCoroutine(RegenMissingHealth(missingHealth));
        }

        if (canReset)
        {
            // Set Bool
            canReset = false;

            // Reset Cast Bar
            castBar.fillAmount = 0;

            // Animate
            enemyAnimator.Play("Wander");

            // Reset for Wander State
            attemptsCount = 0;

            // Drop Target
            playerInRange = false;

            // Drop Target
            target = null;

            // Check if the enemy has reached the destination
            StartCoroutine(CheckReachedDestination(startingPosition));
        }
    }

    IEnumerator RegenMissingHealth(float missingHealth)
    {
        int regenInterval = 1;
        int regenAmount = 5;
        float regeneratedHealth = 0f;

        while (regeneratedHealth < missingHealth && Health < MaxHealth)
        {
            yield return new WaitForSeconds(regenInterval);

            regeneration.Regenerate(regenAmount, regenInterval);

            regeneratedHealth += regenAmount;
        }

        isRegenerating = false;
    }

    protected virtual void HurtState()
    {

    }

    protected virtual void DeathState()
    {
        if (canDeath)
        {
            // Bool
            canDeath = false;

            // Event
            OnDeath?.Invoke();

            // Animator
            enemyAnimator.Play("Death");

            // Components
            enemyCollider2D.enabled = false;
            shadow.SetActive(false);

            if (EnemySpawner != null)
            {
                EnemySpawner.DecreaseEnemyCount();
            }

            if (target != null)
            {
                LevelSystem levelSystem = target.GetComponent<LevelSystem>();
                if (levelSystem)
                {
                    levelSystem.GainExperienceFlatRate(expToGive);
                }
            }

            // Target
            target = null;
            playerInRange = false;

            this.enabled = false;

            StartCoroutine(DeathDelay());
        }
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(.8f);

        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (enemyState == EnemyState.Death)
        {
            return;
        }

        // Calculate damage after applying armor
        float damageAfterArmor = Mathf.Max(damage - CurrentArmor, 0);

        // Apply the reduced damage to the player's health
        Health -= damageAfterArmor;

        idleTime = 0;

        StartCoroutine(healthBar.FlashEffect(Color.red));
        healthBar.ShowFloatingText(damageAfterArmor, healthBar.DamageText);

        OnHealthChanged?.Invoke();

        if (Health <= 0)
        {
            enemyState = EnemyState.Death;
        }
    }

    public void Heal(float heal)
    {
        if (Health >= MaxHealth)
        {
            healthBar.ShowFloatingText(0, healthBar.HealText);

            return;
        }

        float newHealth = Health + heal;

        if (newHealth <= MaxHealth)
        {
            Health += heal;

            idleTime = 0;

            StartCoroutine(healthBar.FlashEffect(Color.green));
            healthBar.ShowFloatingText(heal, healthBar.HealText);

            OnHealthChanged?.Invoke();
        }
        else
        {
            float overheal = newHealth - MaxHealth;
            Health += overheal;

            idleTime = 0;

            StartCoroutine(healthBar.FlashEffect(Color.green));
            healthBar.ShowFloatingText(overheal, healthBar.HealText);

            OnHealthChanged?.Invoke();
        }
    }

    #region Crowd Control

    public void Immobilize()
    {
        if (Immobilized)
        {
            enemyRB.velocity = Vector2.zero;

            Immobilized = true;
        }
    }

    protected virtual void HandleInterrupt()
    {

    }

    protected virtual void Interrupt()
    {
        if (castBar.color == Color.white)
        {
            StartCoroutine(InterruptCastBar());

            // State Transition
            enemyState = EnemyState.Idle;
            return;
        }
    }

    #endregion

    #region Cast Bar

    public void UpdateCastBar(float castTime, float attackCastTime)
    {
        if (castBar != null)
        {
            if (castBar.color == Color.yellow)
            {
                // Increase cast bar time once per second
                castBarTime += Time.deltaTime;

                // Calculate the percentage of the cast time that has elapsed
                float fillAmount = Mathf.Clamp01(castTime / attackCastTime);

                // Set the fillAmount of the cast bar
                castBar.fillAmount = fillAmount;
            }
        }
    }

    public IEnumerator InterruptCastBar()
    {
        if (castBar != null)
        {
            // Set Cast Bar Color
            castBar.color = Color.red;

            // Reset Timer
            castBarTime = 0;

            // Reset Timer
            impactTime = 0f;

            // Reset Timer
            recoveryTime = 0f;

            // Reset Bool
            hasAttacked = false;

            yield return new WaitForSeconds(.2f);

            // Reset Fill
            castBar.fillAmount = 0;
        }
    }

    #endregion

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

    #region Triggers

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
            //target = null;
            //playerInRange = false;
        }
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startingPosition, wanderRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, mobilityRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, specialRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(startingPosition, deAggroRadius);
    }
}
