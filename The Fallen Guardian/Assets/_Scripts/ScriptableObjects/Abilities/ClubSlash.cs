using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/ClubSlash")]
public class ClubSlash : ScriptableObject, IBasicAttackBehaviour
{
    [SerializeField] GameObject clubSlash;

    // Standard Variables
    [SerializeField] int damage;
    [SerializeField] float coolDown;
    [SerializeField] float castTime;

    // Slide Variables
    [SerializeField] float rangeBeforeSlide;
    [SerializeField] float slideForce;
    [SerializeField] float slideDuration;

    [SerializeField] float knockBackForce;

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
        // .3 seconds is the amout of Anticipation time before Impact
        yield return new WaitForSeconds(castTime);

        GameObject slash = Instantiate(clubSlash, stateMachine.transform.position, stateMachine.AttackDir);
        Physics2D.IgnoreCollision(slash.GetComponent<Collider2D>(), stateMachine.gameObject.GetComponent<Collider2D>());
        stateMachine.HandleSlideForward(stateMachine.AttackDir.eulerAngles.z, rangeBeforeSlide);

        // Damage
        slash.GetComponent<DamageOnTrigger>().abilityDamage = damage;
        slash.GetComponent<DamageOnTrigger>().playerDamage = stateMachine.Player.characterStats.damage;

        // Knockback
        slash.GetComponent<KnockbackOnTrigger>().knockBackForce = knockBackForce;
    }

    IEnumerator DurationOfBasicAttack(PlayerStateMachine stateMachine)
    {
        yield return new WaitForSeconds(coolDown);

        stateMachine.SetState(new PlayerIdleState(stateMachine));

        stateMachine.CanBasicAttack = true;
    }
}
