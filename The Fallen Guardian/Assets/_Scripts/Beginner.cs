using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beginner : Player
{
    [SerializeField] GameObject Club;
    [SerializeField] Animator clubAnimator;

    [SerializeField] bool isClubEquipt;

    [SerializeField] GameObject clubSlash;

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

    protected override void HurtState()
    {
        base.HurtState();
        clubAnimator.Play("Hurt");
    }

    protected override void BasicAttackState()
    {
        if (canBasicAttack)
        {
            canBasicAttack = false;

            bodyAnimator.Play("Sword Basic Attack");
            clubAnimator.Play("Basic Attack");

            FaceAttackingDirection(bodyAnimator);
            FaceAttackingDirection(clubAnimator);

            StartCoroutine(AttackImpact());
            StartCoroutine(DurationOfBasicAttack());
        }
    }

    IEnumerator AttackImpact()
    {
        yield return new WaitForSeconds(.3f);

        Instantiate(clubSlash, transform.position, aimer.rotation);

        canSlideForward = true;
    }

    IEnumerator DurationOfBasicAttack()
    {
        yield return new WaitForSeconds(.8f);

        // If we are interrupted this should prevent issues with state changes
        if (state == PlayerState.BasicAttack)
        {
            state = PlayerState.Idle;
        }

        canBasicAttack = true;
    }
}
