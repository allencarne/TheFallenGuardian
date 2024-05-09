using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IDamageable
{
    // Stats
    public PlayerStats Stats;
    public PlayerAbilities playerAbilities;

    // Healthbar
    [SerializeField] HealthBar healthBar;

    // Events
    public UnityEvent OnHealthChanged;
    public UnityEvent OnPlayerDeath;

    // Status Effects
    [HideInInspector] public Buffs Buffs;
    [HideInInspector] public Debuffs debuffs;
    [HideInInspector] public CrowdControl CrowdControl;

    // Combat
    public bool InCombat = false;
    public float IdleTime;
    public float LoseCombatTime;

    // Regen
    private float regenTimer = 0f;
    private float regenInterval = 3f;

    public UnityEvent OnPlayerEnterCombat;
    public UnityEvent OnPlayerLeaveCombat;

    private void Awake()
    {
        Buffs = GetComponent<Buffs>();
        debuffs = GetComponent<Debuffs>();
        CrowdControl = GetComponent<CrowdControl>();
    }

    private void Start()
    {
        Stats.Gold = 0;

        // Set Health
        Stats.Health = Stats.MaxHealth;

        // Set Speed
        Stats.CurrentSpeed = Stats.BaseSpeed;

        // Set Damage
        Stats.CurrentDamage = Stats.BaseDamage;
    }

    private void Update()
    {
        if (InCombat)
        {
            IdleTime += Time.deltaTime;

            if (IdleTime >= LoseCombatTime)
            {
                PlayerLeaveCombat();
            }
        }
        else
        {
            if (Stats.Health < Stats.MaxHealth)
            {
                Buffs.IsRegeneration = true;
                Buffs.Regeneration();
                regenTimer += Time.deltaTime;

                if (regenTimer >= regenInterval)
                {
                    Regeneration(1); // Heal by 1
                    regenTimer = 0; // Reset the timer after healing
                }
            }
            else
            {
                Buffs.IsRegeneration = false;
                Buffs.Regeneration();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Stats.Health -= damage;

        StartCoroutine(healthBar.FlashEffect(Color.red));
        healthBar.ShowFloatingText(damage, Color.red);

        OnHealthChanged?.Invoke();

        PlayerEnterCombat();

        if (Stats.Health <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    public void Heal(float heal)
    {
        if (Stats.Health >= Stats.MaxHealth)
        {
            healthBar.ShowFloatingText(0, Color.green);

            return;
        }

        float newHealth = Stats.Health + heal;

        if (newHealth <= Stats.MaxHealth)
        {
            Stats.Health += heal;

            StartCoroutine(healthBar.FlashEffect(Color.green));
            healthBar.ShowFloatingText(heal, Color.green);

            PlayerEnterCombat();

            OnHealthChanged?.Invoke();
        }
        else
        {
            float overheal = newHealth - Stats.MaxHealth;
            Stats.Health += overheal;

            StartCoroutine(healthBar.FlashEffect(Color.green));
            healthBar.ShowFloatingText(overheal, Color.green);

            PlayerEnterCombat();

            OnHealthChanged?.Invoke();
        }
    }

    public void Regeneration(float heal)
    {
        if (Stats.Health >= Stats.MaxHealth)
        {
            healthBar.ShowFloatingText(0, Color.green);

            return;
        }

        float newHealth = Stats.Health + heal;

        if (newHealth <= Stats.MaxHealth)
        {
            Stats.Health += heal;

            StartCoroutine(healthBar.FlashEffect(Color.green));
            healthBar.ShowFloatingText(heal, Color.green);

            OnHealthChanged?.Invoke();
        }
        else
        {
            float overheal = newHealth - Stats.MaxHealth;
            Stats.Health += overheal;

            StartCoroutine(healthBar.FlashEffect(Color.green));
            healthBar.ShowFloatingText(overheal, Color.green);

            OnHealthChanged?.Invoke();
        }
    }

    public void PlayerEnterCombat()
    {
        IdleTime = 0;

        InCombat = true;

        OnPlayerEnterCombat?.Invoke();
    }

    public void PlayerLeaveCombat()
    {
        IdleTime = 0;

        InCombat = false;

        OnPlayerLeaveCombat?.Invoke();
    }
}
