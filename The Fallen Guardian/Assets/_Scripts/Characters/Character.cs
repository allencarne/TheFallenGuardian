using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable, IKnockbackable
{
    public CharacterStats characterStats;

    [SerializeField] SpriteRenderer spriteRenderer;
    float flashDuration = 0.1f;

    public void TakeDamage(int damage)
    {
        characterStats.health -= damage;

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
        // Calculate the direction from the opponent's position to your position and normalize it
        Vector2 direction = (opponentPosition - yourPosition).normalized;

        // Apply the knockback force to the opponent's Rigidbody in the calculated direction
        opponentRB.velocity = direction * knockBackAmount;

        // Start a coroutine to handle the duration of the knockback effect
        StartCoroutine(KnockBackDuration(opponentRB));
    }

    IEnumerator KnockBackDuration(Rigidbody2D opponentRB)
    {
        yield return new WaitForSeconds(.3f);

        opponentRB.velocity = Vector2.zero;
    }
}
