using UnityEngine;

public interface IKnockbackable
{
    void KnockBack(Rigidbody2D opponentRB, float knockBackAmount, float knockBackDuration, Vector2 knockBackDirection);
}
