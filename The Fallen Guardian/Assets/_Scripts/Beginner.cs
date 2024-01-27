using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beginner : Player
{

    protected override void BasicAttackState()
    {
        if (canBasicAttack)
        {
            canBasicAttack = false;

            bodyAnimator.Play("Sword Basic Attack");

            canSlide = true;

            StartCoroutine(DurationOfBasicAttack());
        }
    }

    IEnumerator DurationOfBasicAttack()
    {
        yield return new WaitForSeconds(.8f);

        state = PlayerState.Idle;

        canBasicAttack = true;
    }
}
