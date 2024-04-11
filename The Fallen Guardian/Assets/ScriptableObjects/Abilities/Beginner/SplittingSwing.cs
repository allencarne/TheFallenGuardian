using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Beginner/SplittingSwing")]
public class SplittingSwing : ScriptableObject, IAbilityBehaviour
{
    [Header("Setup")]
    public Sprite icon;
    [SerializeField] GameObject SlashPrefab;
    [SerializeField] GameObject hitEffect;

    [Header("Stats")]
    [SerializeField] int damage;
    [SerializeField] float coolDown;
    [SerializeField] float castTime;
    [SerializeField] float attackRange;

    [Header("Slide")]
    [SerializeField] float slideForce;
    [SerializeField] float slideDuration;
    [SerializeField] float rangeBeforeSlide;

    [Header("Knockback")]
    [SerializeField] float knockBackForce;
    [SerializeField] float knockBackDuration;

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
        GameObject slash = Instantiate(SlashPrefab, stateMachine.transform.position + offset, stateMachine.AbilityDir);

        // Ignore collision between the attack and the player
        Physics2D.IgnoreCollision(slash.GetComponent<Collider2D>(), stateMachine.gameObject.GetComponent<Collider2D>());

        // Handle sliding forward
        stateMachine.HandleSlideForward(stateMachine.AbilityDir.eulerAngles.z, rangeBeforeSlide, slideForce, slideDuration);

        // Set damage values for the attack
        DamageOnTrigger damageOnTrigger = slash.GetComponent<DamageOnTrigger>();
        if (damageOnTrigger != null)
        {
            damageOnTrigger.AbilityDamage = damage;
            damageOnTrigger.CharacterDamage = stateMachine.Player.Stats.BaseDamage;
            damageOnTrigger.HitEffect = hitEffect;
        }

        KnockbackOnTrigger knockbackOnTrigger = slash.GetComponent<KnockbackOnTrigger>();
        if (knockbackOnTrigger != null)
        {
            knockbackOnTrigger.KnockBackForce = knockBackForce;
            knockbackOnTrigger.KnockBackDuration = knockBackDuration;
            knockbackOnTrigger.KnockBackDirection = direction;
        }
    }

    IEnumerator DurationOfBasicAttack(PlayerStateMachine stateMachine)
    {
        yield return new WaitForSeconds(coolDown);

        stateMachine.SetState(new PlayerIdleState(stateMachine));

        stateMachine.CanBasicAbility = true;
    }
}
