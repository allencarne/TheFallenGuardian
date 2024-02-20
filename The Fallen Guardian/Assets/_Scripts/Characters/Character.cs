using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable, IKnockbackable
{
    [SerializeField] SpriteRenderer spriteRenderer;
    float flashDuration = 0.1f;

    public void TakeDamage(int damage)
    {
        //health =- damage;

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
