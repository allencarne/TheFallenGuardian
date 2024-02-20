using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/ClubSlash")]
public class ClubSlash : ScriptableObject, IBasicAttackBehaviour
{
    [SerializeField] GameObject clubSlash;
    [SerializeField] int damage;
    [SerializeField] float slideRange;

    public void BehaviourUpdate(PlayerStateMachine stateMachine)
    {
        if (stateMachine.CanBasicAttack)
        {
            stateMachine.AttackDir = stateMachine.Aimer.rotation;

            stateMachine.CanBasicAttack = false;

            stateMachine.BodyAnimator.Play("Sword Basic Attack");
            stateMachine.ClubAnimator.Play("Basic Attack");

            stateMachine.FaceAttackingDirection(stateMachine.BodyAnimator);
            stateMachine.FaceAttackingDirection(stateMachine.ClubAnimator);

            stateMachine.StartCoroutine(AttackImpact(stateMachine));
            stateMachine.StartCoroutine(DurationOfBasicAttack(stateMachine));
        }
    }

    IEnumerator AttackImpact(PlayerStateMachine stateMachine)
    {
        yield return new WaitForSeconds(.3f);

        GameObject slash = Instantiate(clubSlash, stateMachine.transform.position, stateMachine.AttackDir);
        Physics2D.IgnoreCollision(slash.GetComponent<Collider2D>(), stateMachine.gameObject.GetComponent<Collider2D>());
        /*
        if (state == PlayerState.BasicAttack)
        {
            GameObject slash = Instantiate(clubSlash, stateMachine.transform.position, stateMachine.AttackDir);
            Physics2D.IgnoreCollision(slash.GetComponent<Collider2D>(), stateMachine.gameObject.GetComponent<Collider2D>());
            //slash.GetComponent<DamageOnTrigger>().playerDamage = 1;
            //slash.GetComponent<DamageOnTrigger>().abilityDamage = 1;

            //stateMachine.HandleSlideForward(attackDir.eulerAngles.z);
        }
        */
    }

    IEnumerator DurationOfBasicAttack(PlayerStateMachine stateMachine)
    {
        yield return new WaitForSeconds(.8f);

        /*
        // If we are interrupted this should prevent issues with state changes
        if (state == PlayerState.BasicAttack)
        {
            state = PlayerState.Idle;
        }
        */

        stateMachine.SetState(new PlayerIdleState(stateMachine));

        stateMachine.CanBasicAttack = true;
    }
}
