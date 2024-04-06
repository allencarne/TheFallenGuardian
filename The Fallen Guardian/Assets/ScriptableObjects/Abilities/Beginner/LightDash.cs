using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Beginner/LightDash")]
public class LightDash : ScriptableObject, IAbilityBehaviour
{
    public Sprite icon;

    [SerializeField] int damage;
    [SerializeField] float coolDown;
    [SerializeField] float castTime;

    [SerializeField] float dashForce;
    [SerializeField] float dashDuration;

    public void BehaviourUpdate(PlayerStateMachine stateMachine)
    {
        if (stateMachine.canMobilityAbility)
        {
            Debug.Log("Test");

            stateMachine.AbilityDir = stateMachine.Aimer.rotation;

            // Calculate direction based on Aimer rotation
            float angle = stateMachine.Aimer.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            stateMachine.canMobilityAbility = false;

            // Use the calculated direction for handling animations
            //stateMachine.HandleAnimation(stateMachine.BodyAnimator, "Player_Sword", "Attack", direction);
            //stateMachine.HandleAnimation(stateMachine.SwordAnimator, "Sword", "Attack", direction);

            stateMachine.StartCoroutine(DurationOfBasicAttack(stateMachine));
        }
    }

    IEnumerator DurationOfBasicAttack(PlayerStateMachine stateMachine)
    {
        yield return new WaitForSeconds(coolDown);

        stateMachine.SetState(new PlayerIdleState(stateMachine));

        stateMachine.CanBasicAbility = true;
    }
}
