using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;   

//public enum EnemyState
//{
//    Idle,
//    Patrol,
//    Trace,
//    Return,
//    Attack,
//    Damaged,
//    Die,
//}

public abstract class Enemy : MonoBehaviour, IDamageable, IPoolable
{
    [Header("Base Stats")]
    public int CurrentPatrolIndex = 0;
    public float IdleCoolTime = 3f;
    public float IdleElapsedTime = 0f;
    public float MoveSpeed = 3.3f;
    public float FindDistance = 5f;
    public float AttackDistance = 2.5f;
    public float ReturnDistance = 10f;
    public float AttackCooltime = 2f;
    public float _attackElapsedTime = 0f;
    public int Health = 100;
    public int MaxHealth = 100;
    public float DamagedTime = 0.5f;
    public float DeathTime = 2f;

    protected StateMachine<Enemy> stateMachine;
    public IState<Enemy> CurrentState => stateMachine.CurrentState;

    public Vector3 StartPosition;
    protected GameObjectPool<Enemy> _thisPool;
    public NavMeshAgent agent;
    public CharacterController CharacterController;
    public GameObject player;

    public Action OnHealthChanged;

    protected virtual void Start()
    {
        InitializeEnemy();
    }

    protected virtual void InitializeEnemy()
    {
        StartPosition = transform.position;
        stateMachine = new StateMachine<Enemy>(this);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = MoveSpeed;
        CharacterController = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(IState<Enemy> newState)
    {
        stateMachine.ChangeState(newState);
    }

    public virtual void TakeDamage(Damage damage)
    {
        if (stateMachine.CurrentState is DieState) return;
        Health -= damage.Value;
        OnHealthChanged?.Invoke();
        
        if(Health <= 0)
        {
            ChangeState(new DieState());
            return;
        }

        ChangeState(new DamagedState());
    }

    protected IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(DeathTime);
        ReturnToPool();
    }

    public void SetPoolReference<T>(GameObjectPool<T> pool) where T : MonoBehaviour, IPoolable
    {
        _thisPool = pool as GameObjectPool<Enemy>;
    }

    public void ReturnToPool()
    {
        _thisPool?.ReturnToPool(this);
    }

    public abstract Vector3 GetPatrolPosition();
    public abstract int GetPatrolPositionsCount();
}
