using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Enemy
{
    protected override void IdleState()
    {
        enemyAnimator.Play("Idle");

        if (enemyRB.position != startingPosition)
        {
            idleTime += 1 * Time.deltaTime;
        }

        if (idleTime >= 5)
        {
            enemyState = EnemyState.Reset;
        }
    }

    protected override void ResetState()
    {
        // Animation
        enemyAnimator.Play("Reset");

        idleTime = 0;

        StartCoroutine(ResetDuration());
    }

    IEnumerator ResetDuration()
    {
        yield return new WaitForSeconds(.6f);

        enemyRB.position = startingPosition;
        enemyState = EnemyState.Spawn;
    }

    protected override void HurtState()
    {
        /*
        if (isHurt)
        {
            isHurt = false;

            enemyAnimator.Play("Hurt", -1, 0f);

            idleTime = 0;

            StartCoroutine(HurtDuration());
        }
        */
    }

    IEnumerator HurtDuration()
    {
        yield return new WaitForSeconds(.4f);

        enemyState = EnemyState.Idle;
    }
}
