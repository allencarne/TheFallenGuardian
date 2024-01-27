using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beginner : Player
{
    protected override void BasicAttackState()
    {
        bodyAnimator.Play("Sword Basic Attack");
    }
}
