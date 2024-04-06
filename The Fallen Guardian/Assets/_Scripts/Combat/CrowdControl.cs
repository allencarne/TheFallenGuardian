using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour, IKnockbackable
{
    public bool isImmobilized;
    public bool isInterrupted;
    public bool isDisarmed;

    public void Interrupt()
    {
        StartCoroutine(InterruptDelay());
    }

    IEnumerator InterruptDelay()
    {
        isInterrupted = true;

        yield return new WaitForSeconds(.3f);

        isInterrupted = false;
    }

    public void Immobilize(float duration)
    {
        StartCoroutine(ImmobilizeDuration(duration));
    }

    IEnumerator ImmobilizeDuration(float duration)
    {
        isImmobilized = true;

        yield return new WaitForSeconds(duration);

        isImmobilized = false;
    }

    public void Disarm(float duration)
    {
        StartCoroutine(DisarmDuration(duration));
    }

    IEnumerator DisarmDuration(float duration)
    {
        isDisarmed = true;

        yield return new WaitForSeconds(duration);

        isDisarmed = false;
    }

    public void KnockBack(Rigidbody2D opponentRB, float knockBackAmount, float knockBackDuration, Vector2 knockBackDirection)
    {
        Interrupt();
        Immobilize(knockBackDuration);
        Disarm(knockBackDuration);

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
