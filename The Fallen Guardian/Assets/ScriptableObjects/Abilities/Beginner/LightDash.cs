using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Beginner/LightDash")]
public class LightDash : ScriptableObject, IAbilityBehaviour
{
    public Sprite icon;

    [SerializeField] int damage;
    public float coolDown;
    public float coolDownTime;
    [SerializeField] float castTime;

    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;
    [SerializeField] float rangeBeforeSlide;

    public void BehaviourUpdate(PlayerStateMachine stateMachine)
    {
        if (stateMachine.canMobilityAbility)
        {   
            stateMachine.AbilityDir = stateMachine.Aimer.rotation;

            // Calculate direction based on Aimer rotation
            float angle = stateMachine.Aimer.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            stateMachine.canMobilityAbility = false;

            stateMachine.HandleSlideForward(stateMachine.AbilityDir.eulerAngles.z, rangeBeforeSlide, dashForce, dashDuration);

            // Use the calculated direction for handling animations
            stateMachine.HandleAnimation(stateMachine.BodyAnimator, "Player", "Move", direction);
            stateMachine.HandleAnimation(stateMachine.SwordAnimator, "Sword", "Move", direction);

            // Buff
            stateMachine.Player.Buffs.Immovable(dashDuration);
        }

        coolDownTime += Time.deltaTime;

        if (coolDownTime >= coolDown)
        {
            coolDownTime = 0;

            stateMachine.SetState(new PlayerIdleState(stateMachine));

            stateMachine.canMobilityAbility = true;
        }
    }
}
