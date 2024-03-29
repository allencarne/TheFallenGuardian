using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Beginner/ClubSlash")]
public class ClubSlash : ScriptableObject, IAbilityBehaviour
{
    public Sprite icon;
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
        if (stateMachine.CanBasicAbility)
        {
            stateMachine.AbilityDir = stateMachine.Aimer.rotation;

            // Calculate direction based on Aimer rotation
            float angle = stateMachine.Aimer.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            stateMachine.CanBasicAbility = false;

            // Use the calculated direction for handling animations
            stateMachine.HandleAnimation(stateMachine.BodyAnimator, "Player_Sword", "Attack", direction);
            stateMachine.HandleAnimation(stateMachine.SwordAnimator, "Sword", "Attack", direction);

            stateMachine.StartCoroutine(AttackImpact(stateMachine));
            stateMachine.StartCoroutine(DurationOfBasicAttack(stateMachine));
        }
    }

    IEnumerator AttackImpact(PlayerStateMachine stateMachine)
    {
        yield return new WaitForSeconds(castTime);

        // Calculate the direction of the attack
        Vector3 direction = stateMachine.AbilityDir * Vector3.right;

        // Calculate the offset based on the attackRange and the direction
        Vector3 offset = direction * attackRange;

        // Spawn the attack object with the calculated offset
        GameObject slash = Instantiate(clubSlash, stateMachine.transform.position + offset, stateMachine.AbilityDir);

        // Ignore collision between the attack and the player
        Physics2D.IgnoreCollision(slash.GetComponent<Collider2D>(), stateMachine.gameObject.GetComponent<Collider2D>());

        // Handle sliding forward
        stateMachine.HandleSlideForward(stateMachine.AbilityDir.eulerAngles.z, rangeBeforeSlide, slideForce, slideDuration);

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

        stateMachine.CanBasicAbility = true;
    }
}
