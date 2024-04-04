using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dummy : Enemy
{
    //[SerializeField] Image patienceBar;

    protected override void IdleState()
    {
        enemyAnimator.Play("Idle");

        if (enemyRB.position != startingPosition)
        {
            idleTime += 1 * Time.deltaTime;
        }

        UpdatePatienceBar();

        if (idleTime >= patience)
        {
            enemyState = EnemyState.Reset;
        }
    }

    protected override void ResetState()
    {
        // Animation
        enemyAnimator.Play("Reset");

        idleTime = 0;

        UpdatePatienceBar();

        StartCoroutine(ResetDuration());
    }

    IEnumerator ResetDuration()
    {
        yield return new WaitForSeconds(.6f);

        enemyRB.position = startingPosition;
        enemyState = EnemyState.Spawn;
    }

    void UpdatePatienceBar()
    {
        if (patienceBar != null)
        {
            float fillAmount = Mathf.Clamp01(idleTime / patience);

            patienceBar.fillAmount = fillAmount;
        }
    }
}
