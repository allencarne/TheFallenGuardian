using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable, IKnockbackable
{
    public int PlayerIndex;
    public PlayerStats playerStats;
    public PlayerAbilities playerAbilities;

    HealthBar healthBar;
    [SerializeField] GameObject floatingText;

    private void Awake()
    {
        healthBar = GetComponent<HealthBar>();
    }

    private void Start()
    {
        // Set Health
        playerStats.health = playerStats.maxHealth;

        switch (playerStats.playerClass)
        {
            case PlayerClass.Beginner:
                Debug.Log("Beginner");
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (GetComponent<PlayerInput>().currentControlScheme == "Keyboard")
            {
                TakeDamage(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (GetComponent<PlayerInput>().currentControlScheme == "Keyboard")
            {
                Heal(1);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (GetComponent<PlayerInput>().currentControlScheme == "Gamepad")
            {
                TakeDamage(1);
            }
        }
    }

    [SerializeField] SpriteRenderer spriteRenderer;
    float flashDuration = 0.1f;

    public void TakeDamage(float damage)
    {
        playerStats.health -= damage;

        StartCoroutine(FlashEffect(Color.red));

        healthBar.lerpTimer = 0f;

        ShowFloatingText(damage, Color.red);
    }

    public void Heal(float heal)
    {
        playerStats.health += heal;

        StartCoroutine(FlashEffect(Color.green));

        healthBar.lerpTimer = 0f;

        ShowFloatingText(heal,Color.green);
    }

    private IEnumerator FlashEffect(Color color)
    {
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
        if (floatingText)
        {
            GameObject textPrefab = Instantiate(floatingText, transform.position, Quaternion.identity);
            TextMeshPro textMesh = textPrefab.GetComponentInChildren<TextMeshPro>();
            textMesh.text = amount.ToString();
            textMesh.color = color; // Set the color of the text
            Destroy(textPrefab, 1);
        }
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
