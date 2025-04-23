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

    public float MoveSpeed = 3.3f;
    public float FindDistance = 5f;
    public float AttackDistance = 2.5f;
    public float ReturnDistance = 10f;
    public float AttackCooltime = 2f;
    private float _attackElapsedTime = 0f;

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
            case EnemyState.Damaged:
                {
                    Damaged();
                    break;
                }
            case EnemyState.Die:
                {
                    Die();
                    break;
                }
        }
    }

    // 3. 상태 함수들을 구현한다.
    private void Idle()
    {
        // 행동 : 가만히 있는다.

        // 필요 속성
        // 1. 플레이어 (위치)
        // 2. Find
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Idle -> Trace");
            CurrentState = EnemyState.Trace;
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

    private void Damaged()
    {
        // 행동 : 피격당한다.
    }

    private void Die()
    {
        // 행동 : 죽는다
    }
}
