using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Components")]
    PlayerState state;

    [Header("Player")]
    [SerializeField] Player player;
    public Player Player => player;

    [Header("RigidBody")]
    [SerializeField] Rigidbody2D rb;
    public Rigidbody2D Rigidbody => rb;

    [Header("Animator")]
    [SerializeField] Animator bodyAnimator;
    public Animator BodyAnimator => bodyAnimator;
    // Club Animator Here?

    [Header("InputHandler")]
    [SerializeField] PlayerInputHandler inputHandler;
    public PlayerInputHandler InputHandler => inputHandler;

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

    private void FixedUpdate()
    {
        state.FixedUpdate();
    }

    public void SetState(PlayerState newState) => state = newState;
}