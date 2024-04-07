using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour, IKnockbackable
{
    public bool isImmobilized;
    public bool isInterrupted;
    public bool isDisarmed;

    // KnockBack
    private Vector2 knockBackVelocity;
    private float knockBackDuration;
    private Rigidbody2D opponentRB;

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
        // Assign Variables
        this.opponentRB = opponentRB;
        this.knockBackDuration = knockBackDuration;
        knockBackVelocity = knockBackDirection * knockBackAmount;

        // CC
        Interrupt();
        Immobilize(knockBackDuration);
        Disarm(knockBackDuration);

        // Start a coroutine to handle the knockback duration
        StartCoroutine(KnockBackDuration(knockBackDuration));
    }

    private void FixedUpdate()
    {
        if (opponentRB != null && knockBackDuration > 0)
        {
            opponentRB.velocity = knockBackVelocity;
            knockBackDuration -= Time.fixedDeltaTime;
        }
    }

    IEnumerator KnockBackDuration(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Normalized time
            opponentRB.velocity = Vector2.Lerp(knockBackVelocity, Vector2.zero, t);
            yield return null; // Wait for the next frame
        }

        // Ensure the velocity is exactly zero at the end
        opponentRB.velocity = Vector2.zero;
    }
}
