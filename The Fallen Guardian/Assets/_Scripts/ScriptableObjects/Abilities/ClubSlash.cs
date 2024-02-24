using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/ClubSlash")]
public class ClubSlash : ScriptableObject, IBasicAttackBehaviour
{
    // Icon
    [SerializeField] GameObject clubSlash;

    [SerializeField] int damage;
    [SerializeField] float coolDown;
    [SerializeField] float castTime;

    [SerializeField] float rangeBeforeSlide;
    [SerializeField] float slideForce;
    [SerializeField] float slideDuration;

    [SerializeField] float attackRange;

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
        yield return new WaitForSeconds(castTime);

        // Calculate the direction of the attack
        Vector3 direction = stateMachine.AttackDir * Vector3.right;

        // Calculate the offset based on the attackRange and the direction
        Vector3 offset = direction * attackRange;

        // Spawn the attack object with the calculated offset
        GameObject slash = Instantiate(clubSlash, stateMachine.transform.position + offset, stateMachine.AttackDir);

        // Ignore collision between the attack and the player
        Physics2D.IgnoreCollision(slash.GetComponent<Collider2D>(), stateMachine.gameObject.GetComponent<Collider2D>());

        // Handle sliding forward
        stateMachine.HandleSlideForward(stateMachine.AttackDir.eulerAngles.z, rangeBeforeSlide, slideForce, slideDuration);

        // Set damage values for the attack
        DamageOnTrigger damageOnTrigger = slash.GetComponent<DamageOnTrigger>();
        if (damageOnTrigger != null)
        {
            damageOnTrigger.abilityDamage = damage;
            damageOnTrigger.playerDamage = stateMachine.Player.playerStats.damage;
        }
    }

    IEnumerator DurationOfBasicAttack(PlayerStateMachine stateMachine)
    {
        yield return new WaitForSeconds(coolDown);

        stateMachine.SetState(new PlayerIdleState(stateMachine));

        stateMachine.CanBasicAttack = true;
    }
}
