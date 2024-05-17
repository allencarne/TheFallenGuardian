using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Beginner/FrailSlash")]
public class FrailSlash : ScriptableObject, IAbilityBehaviour
{
    [Header("Setup")]
    public Sprite icon;
    [SerializeField] GameObject SlashPrefab;
    [SerializeField] GameObject hitEffect;

    [Header("Time")]
    [SerializeField] float castTime;
    [SerializeField] float recoveryTime;
    public float coolDown;
    public float coolDownTime;

    [Header("Stats")]
    [SerializeField] int damage;
    [SerializeField] float attackRange;

    [Header("Slide")]
    [SerializeField] float slideForce;
    [SerializeField] float slideDuration;
    [SerializeField] float rangeBeforeSlide;

    [Header("Knockback")]
    [SerializeField] float knockBackForce;
    [SerializeField] float knockBackDuration;

    [Header("Slow")]
    [SerializeField] float slowAmount;
    [SerializeField] float slowDuration;

    bool canImpact = false;

    public void BehaviourUpdate(PlayerStateMachine stateMachine)
    {
        if (stateMachine.CanBasicAbility && !stateMachine.hasAttacked)
        {
            stateMachine.hasAttacked = true;
            stateMachine.CanBasicAbility = false;

            // Get Angle and Direction
            stateMachine.AbilityDir = stateMachine.Aimer.rotation;
            float angle = stateMachine.Aimer.rotation.eulerAngles.z;
            Vector2 direction = stateMachine.HandleDirection(angle);

            // Animate Body
            stateMachine.BodyAnimator.Play("Sword_Attack_C");
            stateMachine.BodyAnimator.SetFloat("Horizontal", direction.x);
            stateMachine.BodyAnimator.SetFloat("Vertical", direction.y);

            // Animate Sword
            stateMachine.SwordAnimator.Play("Sword_Attack_C");
            stateMachine.SwordAnimator.SetFloat("Horizontal", direction.x);
            stateMachine.SwordAnimator.SetFloat("Vertical", direction.y);

            // Timers
            stateMachine.StartCoroutine(AttackImpact(stateMachine));
            stateMachine.StartCoroutine(CoolDown(stateMachine));
        }

        if (canImpact)
        {
            canImpact = false;

            stateMachine.BodyAnimator.Play("Sword_Attack_R");
            stateMachine.SwordAnimator.Play("Sword_Attack_R");

            stateMachine.StartCoroutine(RecoveryTime(stateMachine));
        }
    }

    IEnumerator AttackImpact(PlayerStateMachine stateMachine)
    {
        float modifiedCastTime = castTime / stateMachine.Player.Stats.CurrentAttackSpeed;

        yield return new WaitForSeconds(modifiedCastTime);

        stateMachine.BodyAnimator.Play("Sword_Attack_I");
        stateMachine.SwordAnimator.Play("Sword_Attack_I");

        stateMachine.StartCoroutine(ImpactDelay());

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
            damageOnTrigger.CharacterDamage = stateMachine.Player.Stats.CurrentDamage;
            damageOnTrigger.HitEffect = hitEffect;
        }

        KnockbackOnTrigger knockbackOnTrigger = slash.GetComponent<KnockbackOnTrigger>();
        if (knockbackOnTrigger != null)
        {
            knockbackOnTrigger.KnockBackForce = knockBackForce;
            knockbackOnTrigger.KnockBackDuration = knockBackDuration;
            knockbackOnTrigger.KnockBackDirection = direction;
        }

        SlowOnTrigger slowOnTrigger = slash.GetComponent<SlowOnTrigger>();
        if (slowOnTrigger != null)
        {
            slowOnTrigger.SlowAmount = slowAmount;
            slowOnTrigger.SlowDuration = slowDuration;
        }
    }

    IEnumerator ImpactDelay()
    {
        yield return new WaitForSeconds(.1f);

        canImpact = true;
    }

    IEnumerator RecoveryTime(PlayerStateMachine stateMachine)
    {
        float modifiedRecoveryTime = recoveryTime / stateMachine.Player.Stats.CurrentAttackSpeed;

        yield return new WaitForSeconds(modifiedRecoveryTime);

        stateMachine.hasAttacked = false;
        stateMachine.SetState(new PlayerIdleState(stateMachine));
    }

    IEnumerator CoolDown(PlayerStateMachine stateMachine)
    {
        // Adjust cooldown time based on cooldown reduction
        float modifiedCooldown = coolDown / stateMachine.Player.Stats.CurrentCDR;

        yield return new WaitForSeconds(modifiedCooldown);

        stateMachine.CanBasicAbility = true;
    }
}
