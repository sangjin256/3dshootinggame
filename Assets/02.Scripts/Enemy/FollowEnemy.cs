using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FollowEnemy : Enemy
{
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed;
        CurrentState = EnemyState.Trace;
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Trace:
                {
                    Trace();
                    break;
                }
            case EnemyState.Attack:
                {
                    Attack();
                    break;
                }
            default:
                {
                    Trace();
                    break;
                }
        }
    }

    private new void Trace()
    {
        // 전이 : 공격 범위 만큼 가까워 지면 -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // 행동 : 플레이어를 추적한다.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position);
    }

    protected new void Attack()
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
        if (_attackElapsedTime >= AttackCooltime)
        {
            Debug.Log("플레이어 공격!");
            _attackElapsedTime = 0f;
        }
    }
}
