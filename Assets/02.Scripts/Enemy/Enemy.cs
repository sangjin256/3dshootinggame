using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;   

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
    public GameObject ExplosionPrefab;

    public float FlashDuration = 0.1f;
    private Coroutine FlashCoroutine;
    public Renderer Renderer;
    private MaterialPropertyBlock PropBlock;

    public GameObject CoinPrefab;

    public IState<Enemy> CurrentState => stateMachine.CurrentState;

    protected Vector3 _startPosition;
    protected GameObjectPool<Enemy> _thisPool;
    protected NavMeshAgent _agent;
    protected CharacterController _characterController;
    public Animator _animator;

    public Action OnHealthChanged;

    protected virtual void Start()
    {
        InitializeEnemy();
    }

    protected virtual void InitializeEnemy()
    {
        PropBlock = new MaterialPropertyBlock();
        _startPosition = transform.position;
        stateMachine = new StateMachine<Enemy>(this);
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
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
        TriggerHitFlash();

        OnHealthChanged?.Invoke();
        
        if(Health <= 0)
        {
            SpawnCoins();
            ChangeState(new DieState());
            return;
        }

        ChangeState(new DamagedState());
    }

    private void SpawnCoins()
    {
        int coinCount = UnityEngine.Random.Range(5, 11); // 5에서 10개 사이의 코인 생성
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 randomOffset = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                0,
                UnityEngine.Random.Range(-1f, 1f)
            );
            
            GameObject coin = Instantiate(CoinPrefab, transform.position + randomOffset, Quaternion.identity);
            Rigidbody rb = coin.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * UnityEngine.Random.Range(5f, 8f), ForceMode.Impulse);
                rb.AddTorque(new Vector3(
                    UnityEngine.Random.Range(-5f, 5f),
                    UnityEngine.Random.Range(-5f, 5f),
                    UnityEngine.Random.Range(-5f, 5f)
                ), ForceMode.Impulse);
            }
        }
    }

    protected IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(DeathTime);
        ReturnToPool();
    }

    public void TriggerHitFlash()
    {
        if (FlashCoroutine != null)
            StopCoroutine(FlashCoroutine);

        FlashCoroutine = StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash()
    {
        Flash(1);
        yield return new WaitForSeconds(FlashDuration);
        Flash(0);
    }

    private void Flash(float value)
    {
        Renderer.GetPropertyBlock(PropBlock);
        PropBlock.SetFloat("_FlashAmount", value);
        Renderer.SetPropertyBlock(PropBlock);
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

    public void Move(Vector3 position) => _agent.SetDestination(position);
    public float GetMinDistance() => _characterController.minMoveDistance;
    public float GetRemainingDistance() => _agent.remainingDistance;
    public Vector3 GetStartPosition() => _startPosition;
    public void SetIsStopped(bool IsStop) => _agent.isStopped = IsStop;
    public void ResetPath() => _agent.ResetPath();
    public Animator GetAnimator() => _animator;
}
