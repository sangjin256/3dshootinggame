using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 인공지능 : 사람처럼 똑똑하게 행동하는 알고리즘
    // - 반응형 / 계획형 -> 규칙 기반 인공지능 (전통적인 방식)
    //                  ->     ㄴ 제어문 (조건문, 반복문)으로 만들어짐

    // 1. 상태를 열거형으로 정의
    public enum EnemyState
    {
        Idle,
        Patrol,
        Trace,
        Return,
        Attack,
        Damaged,
        Die,
    }

    // 2. 현재 상태를 지정
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;
    private CharacterController _characterController;
    private Vector3 _startPosition;
    private Vector3[] PatrolPositionArr = { new Vector3(-11, 1.08f, 2), new Vector3(-11, 1.08f, 18), new Vector3(6, 1.08f, 18), new Vector3(6, 1.08f, 2)};

    public int CurrentPatrolIndex           = 0;
    public float IdleCoolTime               = 3f;
    public float IdleElapsedTime            = 0f;
    public float MoveSpeed                  = 3.3f;
    public float FindDistance               = 5f;
    public float AttackDistance             = 2.5f;
    public float ReturnDistance             = 10f;
    public float AttackCooltime             = 2f;
    private float _attackElapsedTime        = 0f;
    public int Health                       = 100;
    public float DamagedTime                = 0.5f;
    public float DeathTime                  = 2f;


    private void Start()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
                {
                    Idle();
                    break;
                }
            case EnemyState.Patrol:
                {
                    Patrol();
                    break;
                }
            case EnemyState.Trace:
                {
                    Trace();
                    break;
                }
            case EnemyState.Return:
                {
                    Return();
                    break;
                }
            case EnemyState.Attack:
                {
                    Attack();
                    break;
                }
        }
    }

    public void TakeDamage(Damage damage)
    {
        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die) return;

        Health -= damage.Value;

        if(Health <= 0)
        {
            CurrentState = EnemyState.Die;
            Debug.Log($"상태전환 : {CurrentState} -> Die");
            CurrentState = EnemyState.Die;
            StartCoroutine(Die_Coroutine());
            return;
        }

        Debug.Log($"상태전환 : {CurrentState} -> Damaged");

        CurrentState = EnemyState.Damaged;
        StartCoroutine(Damagaed_Coroutine());
    }

    // 3. 상태 함수들을 구현한다.
    private void Idle()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Idle -> Trace");
            IdleElapsedTime = 0f;
            CurrentState = EnemyState.Trace;
        }

        IdleElapsedTime += Time.deltaTime;
        if (IdleElapsedTime >= IdleCoolTime)
        {
            Debug.Log("상태전환 : Idle -> Patrol");
            IdleElapsedTime = 0f;
            CurrentPatrolIndex = 0;
            CurrentState = EnemyState.Patrol;
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Patrol -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        Vector3 dir = (PatrolPositionArr[CurrentPatrolIndex] - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        Debug.Log(Vector3.Distance(transform.position, PatrolPositionArr[CurrentPatrolIndex]));
        if (Vector3.Distance(transform.position, PatrolPositionArr[CurrentPatrolIndex]) <= 0.1f)
        {
            CurrentPatrolIndex++;
            if (CurrentPatrolIndex >= PatrolPositionArr.Length) CurrentPatrolIndex = 0;
        }
    }

    private void Trace()
    {
        // 전이 : 공격 범위 만큼 가까워 지면 -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }
        // 전이 : 플레이어와 멀어지면 -> Return
        if (Vector3.Distance(transform.position, _player.transform.position) >= ReturnDistance)
        {
            Debug.Log("상태전환 : Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }

        // 행동 : 플레이어를 추적한다.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        // 전이 : 시작 위치와 가까워지면 -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance)
        {
            Debug.Log("상태전환 : Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }
        // 전이 : 중간에 플레이어와 가까워지면 Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Return -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }



        // 행동 : 시작 위치로 되돌아간다.
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // 전이 : 공격 범위보다 멀어지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환 : Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackElapsedTime = 0f;
            return;
        }

        _attackElapsedTime += Time.deltaTime;
        if(_attackElapsedTime >= AttackCooltime)
        {
            Debug.Log("플레이어 공격!");
            _attackElapsedTime = 0f;
        }
    }

    public IEnumerator Damagaed_Coroutine()
    {
        yield return new WaitForSeconds(DamagedTime);
        Debug.Log("상태전환 : Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(DeathTime);
        gameObject.SetActive(false);
    }
}
