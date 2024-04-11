using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public PlayerStats Stats;
    public PlayerAbilities playerAbilities;

    public Image CastBar;
    [SerializeField] GameObject floatingText;
    public bool isPlayerOutOfHealth;

    // Events
    public UnityEvent OnHealthChanged;

    // Status Effects
    public Buffs Buffs;
    Debuffs debuffs;
    public CrowdControl CrowdControl;

    private void Awake()
    {
        Buffs = GetComponent<Buffs>();
        debuffs = GetComponent<Debuffs>();
        CrowdControl = GetComponent<CrowdControl>();
    }

    private void Start()
    {
        // *Temporary* Player Stats
        Stats.PlayerLevel = 1;
        Stats.CurrentExperience = 0;
        Stats.RequiredExperience = 130;

        // *Temporary* Character Stats
        Stats.MaxHealth = 10;
        Stats.BaseDamage = 1;
        Stats.BaseSpeed = 8;


        // Set Health
        Stats.Health = Stats.MaxHealth;

        // Set Speed
        Stats.CurrentSpeed = Stats.BaseSpeed;

        switch (Stats.PlayerClass)
        {
            case PlayerClass.Beginner:
                //Debug.Log("Beginner");
                break;
            case PlayerClass.Warrior:
                //playerAbilities.AttackBehaviour = playerAbilities.WarriorAttackBehaviour;
                break;
            case PlayerClass.Magician:
                break;
            case PlayerClass.Archer:
                break;
            case PlayerClass.Rogue:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = .75f;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Time.timeScale = .5f;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            Time.timeScale = .25f;
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (GetComponent<PlayerInput>().currentControlScheme == "Keyboard")
            {
                TakeDamage(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (GetComponent<PlayerInput>().currentControlScheme == "Keyboard")
            {
                Heal(2);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Stats.Health -= damage;

        StartCoroutine(FlashEffect(Color.red));
        ShowFloatingText(damage, Color.red);

        OnHealthChanged?.Invoke();

        if (Stats.Health <= 0)
        {
            isPlayerOutOfHealth = true;
        }
    }

    public void Heal(float heal)
    {
        if (Stats.Health >= Stats.MaxHealth)
        {
            ShowFloatingText(0, Color.green);

            return;
        }

        float newHealth = Stats.Health + heal;

        if (newHealth <= Stats.MaxHealth)
        {
            Stats.Health += heal;

            StartCoroutine(FlashEffect(Color.green));
            ShowFloatingText(heal, Color.green);

            OnHealthChanged?.Invoke();
        }
        else
        {
            float overheal = newHealth - Stats.MaxHealth;
            Stats.Health += overheal;

            StartCoroutine(FlashEffect(Color.green));
            ShowFloatingText(overheal, Color.green);

            OnHealthChanged?.Invoke();
        }
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

    public void UpdateCastBar(float fillAmount)
    {
        if (CastBar != null)
        {
            CastBar.fillAmount = fillAmount;
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
