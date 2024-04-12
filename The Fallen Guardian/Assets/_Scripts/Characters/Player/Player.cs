using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    private void Awake()
    {
        Buffs = GetComponent<Buffs>();
        debuffs = GetComponent<Debuffs>();
        CrowdControl = GetComponent<CrowdControl>();
    }

    private void Start()
    {
        // Set Health
        Stats.Health = Stats.MaxHealth;

        // Set Speed
        Stats.CurrentSpeed = Stats.BaseSpeed;
    }

    public void TakeDamage(float damage)
    {
        Stats.Health -= damage;

        StartCoroutine(healthBar.FlashEffect(Color.red));
        healthBar.ShowFloatingText(damage, Color.red);

        OnHealthChanged?.Invoke();

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

    public void HandleSlow()
    {
        // Calculate the final speed, ensuring it doesn't drop below 0
        Stats.CurrentSpeed = Mathf.Max(Stats.BaseSpeed - debuffs.SlowAmount, 0);
    }

    public void HandleSlowEnd()
    {
        Stats.CurrentSpeed = Stats.BaseSpeed;
    }
}
