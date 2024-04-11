using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour, IKnockbackable
{
    [SerializeField] Buffs buffs;

    [Header("CC Bar")]
    [SerializeField] GameObject ccBar;

    [Header("CC")]
    [SerializeField] GameObject cc_KnockBack;
    [SerializeField] GameObject cc_Stun;

    [Header("Bools")]
    public bool IsImmobilized;
    public bool IsInterrupted;
    public bool IsDisarmed;

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
        IsInterrupted = true;

        yield return new WaitForSeconds(.3f);

        IsInterrupted = false;
    }

    public void Immobilize(float duration)
    {
        StartCoroutine(ImmobilizeDuration(duration));
    }

    IEnumerator ImmobilizeDuration(float duration)
    {
        IsImmobilized = true;

        yield return new WaitForSeconds(duration);

        IsImmobilized = false;
    }

    public void Disarm(float duration)
    {
        StartCoroutine(DisarmDuration(duration));
    }

    IEnumerator DisarmDuration(float duration)
    {
        IsDisarmed = true;

        yield return new WaitForSeconds(duration);

        IsDisarmed = false;
    }

    public void KnockBack(Rigidbody2D opponentRB, float knockBackAmount, float knockBackDuration, Vector2 knockBackDirection)
    {
        if (buffs.IsImmovable)
        {
            return;
        }

        // Assign Variables
        this.opponentRB = opponentRB;
        this.knockBackDuration = knockBackDuration;
        knockBackVelocity = knockBackDirection * knockBackAmount;

        // CC
        Interrupt();
        Immobilize(knockBackDuration);
        Disarm(knockBackDuration);

        // Icon
        GameObject ccIcon = Instantiate(cc_KnockBack);
        ccIcon.transform.SetParent(ccBar.transform);
        ccIcon.transform.localScale = new Vector3(1,1,1);

        // Start a coroutine to handle the knockback duration
        StartCoroutine(KnockBackDuration(knockBackDuration, ccIcon));
    }

    private void FixedUpdate()
    {
        if (opponentRB != null && knockBackDuration > 0)
        {
            opponentRB.velocity = knockBackVelocity;
            knockBackDuration -= Time.fixedDeltaTime;
        }
    }

    IEnumerator KnockBackDuration(float duration, GameObject ccIcon)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Normalized time
            opponentRB.velocity = Vector2.Lerp(knockBackVelocity, Vector2.zero, t);
            yield return null; // Wait for the next frame
        }

        Destroy(ccIcon);

        // Ensure the velocity is exactly zero at the end
        opponentRB.velocity = Vector2.zero;
    }
}
