using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable, IKnockbackable
{
    [SerializeField] SpriteRenderer spriteRenderer;
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
                Heal(1);
            }
        }
    }

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
