using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Player player;
    PlayerState state;

    [Header("RigidBody")]
    [SerializeField] Rigidbody2D rb;
    public Rigidbody2D Rigidbody => rb;

    [Header("Animator")]
    [SerializeField] Animator bodyAnimator;
    public Animator BodyAnimator => bodyAnimator;

    // Club Animator Here?

    [Header("Abilities")]
    [SerializeField] PlayerAbilities abilities;
    public PlayerAbilities Abilities => abilities;

    private void Awake()
    {
        SetState(new PlayerSpawnState(this));
    }

    void Update()
    {
        state.Update();
    }

    public void SetState(PlayerState newState) => state = newState;
}