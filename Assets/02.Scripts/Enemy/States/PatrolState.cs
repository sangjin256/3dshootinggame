using UnityEngine;

public class PatrolState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("IdleToMove");
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position) < enemy.FindDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        enemy.Move(enemy.GetPatrolPosition());

        if (enemy.GetRemainingDistance() <= enemy.GetMinDistance())
        {
            enemy.CurrentPatrolIndex++;
            if (enemy.CurrentPatrolIndex >= enemy.GetPatrolPositionsCount())
                enemy.CurrentPatrolIndex = 0;
        }
    }

    public void Exit(Enemy enemy)
    {
    }
} 