using UnityEngine;

public interface IKnockbackable
{
    void KnockBack(Vector3 opponentPosition, Vector3 yourPosition, Rigidbody2D opponentRB, float knockBackAmount);
}
