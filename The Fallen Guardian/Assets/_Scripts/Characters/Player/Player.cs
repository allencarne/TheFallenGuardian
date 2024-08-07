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
    [HideInInspector] public CrowdControl CrowdControl;
    public Buff_Immovable Immovable;
    public Buff_Swiftness Swiftness;

    // Combat
    public bool InCombat = false;
    public float IdleTime;
    public float LoseCombatTime;

    // Regen
    Buff_Regeneration regeneration;
    bool isRegenerating = false;

    // Fury
    bool isFuryTimerActive = false;
    bool isDecreasingFury = false;

    public UnityEvent OnPlayerEnterCombat;
    public UnityEvent OnPlayerLeaveCombat;

    private void Awake()
    {
        CrowdControl = GetComponent<CrowdControl>();
        regeneration = GetComponent<Buff_Regeneration>();
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

        // Set Attack Speed
        Stats.CurrentAttackSpeed = Stats.BaseAttackSpeed;

        // Set CDR
        Stats.CurrentCDR = Stats.BaseCDR;

        // Set Armor
        Stats.CurrentArmor = Stats.BaseArmor;

        // Set Fury
        Stats.Fury = 0;
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

        if (!InCombat && !isRegenerating)
        {
            if (Stats.Health < Stats.MaxHealth)
            {
                isRegenerating = true;

                // Calculate Missing Health
                float missingHealth = Stats.MaxHealth - Stats.Health;

                // Start CoRoutine
                StartCoroutine(RegenMissingHealth(missingHealth));
            }
        }
    }

    IEnumerator RegenMissingHealth(float missingHealth)
    {
        int regenInterval = 1;
        int regenAmount = 5;
        float regeneratedHealth = 0f;

        while (regeneratedHealth < missingHealth && Stats.Health < Stats.MaxHealth)
        {
            yield return new WaitForSeconds(regenInterval);

            regeneration.Regenerate(regenAmount, regenInterval);

            regeneratedHealth += regenAmount;
        }

        isRegenerating = false;
    }

    public void TakeDamage(float damage)
    {
        // Calculate damage after applying armor
        float damageAfterArmor = Mathf.Max(damage - Stats.CurrentArmor, 0);

        // Apply the reduced damage to the player's health
        Stats.Health -= damageAfterArmor;

        StartCoroutine(healthBar.FlashEffect(Color.red));
        healthBar.ShowFloatingText(damageAfterArmor, healthBar.floatingDamageText);

        OnHealthChanged?.Invoke();

        PlayerEnterCombat();

        if (Stats.Health <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    public void Heal(float heal, bool enterCombat)
    {
        if (Stats.Health >= Stats.MaxHealth)
        {
            healthBar.ShowFloatingText(0, healthBar.floatingHealingText);

            return;
        }

        float newHealth = Stats.Health + heal;

        if (newHealth <= Stats.MaxHealth)
        {
            Stats.Health += heal;

            StartCoroutine(healthBar.FlashEffect(Color.green));
            healthBar.ShowFloatingText(heal, healthBar.floatingHealingText);

            if (enterCombat)
            {
                PlayerEnterCombat();
            }

            OnHealthChanged?.Invoke();
        }
        else
        {
            float overheal = newHealth - Stats.MaxHealth;
            Stats.Health += overheal;

            StartCoroutine(healthBar.FlashEffect(Color.green));
            healthBar.ShowFloatingText(overheal, healthBar.floatingHealingText);

            PlayerEnterCombat();

            OnHealthChanged?.Invoke();
        }
    }

    int currentSwiftness = 0;

    public void GainFury(float amount)
    {
        // Ensure Fury does not exceed MaxFury
        Stats.Fury = Mathf.Min(Stats.Fury + amount, Stats.MaxFury);

        UpdateSwiftness();

        if (isFuryTimerActive)
        {
            CancelInvoke("FuryTimerExpired");
        }

        if (isDecreasingFury)
        {
            CancelInvoke("DecreaseFury");
            isDecreasingFury = false;
        }

        isFuryTimerActive = true;
        Invoke("FuryTimerExpired", 8.0f);
    }

    private void FuryTimerExpired()
    {
        isFuryTimerActive = false;
        isDecreasingFury = true;
        InvokeRepeating("DecreaseFury", 0f, 1f);
    }

    private void DecreaseFury()
    {
        if (Stats.Fury > 0)
        {
            Stats.Fury -= 1;
            UpdateSwiftness();
        }
        else
        {
            CancelInvoke("DecreaseFury");
            isDecreasingFury = false;
        }
    }

    private void UpdateSwiftness()
    {
        int newSwiftness = 0;

        if (Stats.Fury >= 100)
        {
            newSwiftness = 10;
        }
        else if (Stats.Fury >= 80)
        {
            newSwiftness = 8;
        }
        else if (Stats.Fury >= 60)
        {
            newSwiftness = 6;
        }
        else if (Stats.Fury >= 40)
        {
            newSwiftness = 4;
        }
        else if (Stats.Fury >= 20)
        {
            newSwiftness = 2;
        }

        if (newSwiftness != currentSwiftness)
        {
            int swiftnessDifference = newSwiftness - currentSwiftness;

            if (swiftnessDifference > 0)
            {
                Swiftness.ApplyConditionalSwiftness(swiftnessDifference);
            }
            else
            {
                Swiftness.RemoveConditionalSwiftness(-swiftnessDifference);
            }

            currentSwiftness = newSwiftness;
        }

        // If currentSwiftness is 0, reset Swiftness completely
        if (currentSwiftness == 0)
        {
            Swiftness.ResetSwiftness();
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
