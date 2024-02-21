using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable, IKnockbackable
{
    public int PlayerIndex;
    public PlayerStats playerStats;
    public PlayerAbilities playerAbilities;

    private void Start()
    {
        // Set Health
        playerStats.health = playerStats.maxHealth;

        switch (playerStats.playerClass)
        {
            case PlayerClass.Beginner:
                Debug.Log("Hi");
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

    [SerializeField] SpriteRenderer spriteRenderer;
    float flashDuration = 0.1f;

    public void TakeDamage(int damage)
    {
        playerStats.health -= damage;

        StartCoroutine(FlashOnDamage());

        Debug.Log("TakeDamage" + damage);
    }

    private IEnumerator FlashOnDamage()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration / 2);

        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(flashDuration / 2);

        // Reset to original color
        spriteRenderer.color = Color.white;
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
