using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] protected Animator enemyAnimator;
    [SerializeField] protected Rigidbody2D enemyRB;
    protected Vector2 startingPosition;

    protected float idleTime;

    protected bool isEnemyHurt = false;
    bool canSpawn = true;

    protected enum EnemyState 
    { 
        Spawn,
        Idle,
        Wander,
        Chase,
        Attack,
        Mobility,
        Special,
        Reset,
        Hurt,
        Death
    }

    protected EnemyState enemyState = EnemyState.Spawn;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Spawn:
                SpawnState();
                break;
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.Wander:
                WanderState();
                break;
            case EnemyState.Chase:
                ChaseState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
            case EnemyState.Mobility:
                MobilityState();
                break;
            case EnemyState.Special:
                SpecialState();
                break;
            case EnemyState.Reset:
                ResetState();
                break;
            case EnemyState.Hurt:
                HurtState();
                break;
            case EnemyState.Death:
                DeathState();
                break;
        }
    }

    protected virtual void SpawnState()
    {
        if (canSpawn)
        {
            canSpawn = false;

            enemyAnimator.Play("Spawn");

            StartCoroutine(SpawnDuration());
        }
    }

    IEnumerator SpawnDuration()
    {
        yield return new WaitForSeconds(.6f);

        enemyState = EnemyState.Idle;

        canSpawn = true;
    }

    protected virtual void IdleState()
    {

    }

    protected virtual void WanderState()
    {

    }

    protected virtual void ChaseState()
    {

    }

    protected virtual void AttackState()
    {

    }

    protected virtual void MobilityState()
    {

    }

    protected virtual void SpecialState()
    {

    }

    protected virtual void ResetState()
    {

    }

    protected virtual void HurtState()
    {

    }

    protected virtual void DeathState()
    {

    }
}
