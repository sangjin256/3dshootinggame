using UnityEngine;
using UnityEngine.AI;

public class ElitePatrolState : IState<Enemy>
{
    private float patrolRadius = 15f;
    private float patrolWaitTime = 3f;
    private float currentWaitTime;
    private Vector3 patrolPoint;

    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("IdleToMove");
        currentWaitTime = patrolWaitTime;
        SetNewPatrolPoint(enemy);
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, GameManager.I.Player.transform.position) < enemy.FindDistance)
        {
            enemy.ChangeState(new EliteTraceState());
            return;
        }

        if (enemy.GetRemainingDistance() < 0.5f)
        {
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0)
            {
                SetNewPatrolPoint(enemy);
                currentWaitTime = patrolWaitTime;
            }
        }
    }

    public void Exit(Enemy enemy)
    {
        // 필요한 정리 작업이 있다면 여기에 구현
    }

    private void SetNewPatrolPoint(Enemy enemy)
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection.y = 0;
        patrolPoint = enemy.transform.position + randomDirection;
        enemy.Move(patrolPoint);
    }
} 