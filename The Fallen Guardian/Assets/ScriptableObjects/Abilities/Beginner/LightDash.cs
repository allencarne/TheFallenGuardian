using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Beginner/LightDash")]
public class LightDash : ScriptableObject, IAbilityBehaviour
{
    public UnityEvent OnMobilityCoolDownStarted;

    [Header("Setup")]
    public Sprite icon;
    [SerializeField] GameObject dashEffect;

    [Header("Time")]
    public float CoolDown;

    [Header("Dash")]
    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;
    [SerializeField] float rangeBeforeSlide;

    public void BehaviourUpdate(PlayerStateMachine stateMachine)
    {
        if (stateMachine.canMobilityAbility && !stateMachine.hasAttacked)
        {
            stateMachine.hasAttacked = true;
            stateMachine.canMobilityAbility = false;

            // Get Angle and Direction
            stateMachine.AbilityDir = stateMachine.Aimer.rotation;
            float angle = stateMachine.Aimer.rotation.eulerAngles.z;
            Vector2 direction = stateMachine.HandleDirection(angle);

            // Head
            stateMachine.HeadAnimator.Play(stateMachine.Equipment.HeadIndex + "_Dash");
            stateMachine.HeadAnimator.SetFloat("Horizontal", direction.x);
            stateMachine.HeadAnimator.SetFloat("Vertical", direction.y);

            // Chest
            stateMachine.ChestAnimator.Play(stateMachine.Equipment.ChestIndex + "_Dash");
            stateMachine.ChestAnimator.SetFloat("Horizontal", direction.x);
            stateMachine.ChestAnimator.SetFloat("Vertical", direction.y);

            // Legs
            stateMachine.LegsAnimator.Play(stateMachine.Equipment.LegsIndex + "_Dash");
            stateMachine.LegsAnimator.SetFloat("Horizontal", direction.x);
            stateMachine.LegsAnimator.SetFloat("Vertical", direction.y);

            // Animate Body
            stateMachine.BodyAnimator.Play("Dash");
            stateMachine.BodyAnimator.SetFloat("Horizontal", direction.x);
            stateMachine.BodyAnimator.SetFloat("Vertical", direction.y);

            // Animate Sword
            stateMachine.SwordAnimator.Play("Dash");
            stateMachine.SwordAnimator.SetFloat("Horizontal", direction.x);
            stateMachine.SwordAnimator.SetFloat("Vertical", direction.y);

            // Handle Dash
            stateMachine.HandleSlideForward(stateMachine.AbilityDir.eulerAngles.z, rangeBeforeSlide, dashForce, dashDuration);

            // Spawn Particle
            Instantiate(dashEffect, stateMachine.transform.position, stateMachine.AbilityDir);

            // Buff
            stateMachine.Player.Immovable.Immovable(dashDuration);

            // Timers
            stateMachine.StartCoroutine(CoolDownTime(stateMachine));
            stateMachine.StartCoroutine(RecoveryTime(stateMachine));
        }
    }

    IEnumerator RecoveryTime(PlayerStateMachine stateMachine)
    {
        //float modifiedRecoveryTime = recoveryTime / stateMachine.Player.Stats.CurrentAttackSpeed;

        yield return new WaitForSeconds(dashDuration);

        stateMachine.hasAttacked = false;
        stateMachine.SetState(new PlayerIdleState(stateMachine));
    }

    IEnumerator CoolDownTime(PlayerStateMachine stateMachine)
    {
        OnMobilityCoolDownStarted?.Invoke();

        // Adjust cooldown time based on cooldown reduction
        float modifiedCooldown = CoolDown / stateMachine.Player.Stats.CurrentCDR;

        yield return new WaitForSeconds(modifiedCooldown);

        stateMachine.canMobilityAbility = true;
    }
}
