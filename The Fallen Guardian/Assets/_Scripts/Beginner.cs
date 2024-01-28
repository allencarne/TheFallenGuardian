using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beginner : Player
{
    [SerializeField] GameObject Club;
    [SerializeField] Animator clubAnimator;

    [SerializeField] bool isClubEquipt;

    protected override void IdleState()
    {
        base.IdleState();

        clubAnimator.Play("Idle");
    }

    protected override void MoveState()
    {
        base.MoveState();

        clubAnimator.Play("Move");

        // Set idle Animation after move
        if (inputHandler.MoveInput != Vector2.zero)
        {
            clubAnimator.SetFloat("Horizontal", inputHandler.MoveInput.x);
            clubAnimator.SetFloat("Vertical", inputHandler.MoveInput.y);
        }
    }

    protected override void BasicAttackState()
    {
        if (canBasicAttack)
        {
            canBasicAttack = false;

            bodyAnimator.Play("Sword Basic Attack");
            clubAnimator.Play("Basic Attack");

            canSlideForward = true;

            FaceAttackingDirection(bodyAnimator);
            FaceAttackingDirection(clubAnimator);

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
