using UnityEngine;

public class PatrolState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.FindDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        Vector3 dir = (enemy.GetPatrolPosition() - enemy.transform.position).normalized;
        enemy.CharacterController.Move(dir * enemy.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(enemy.transform.position, enemy.GetPatrolPosition()) <= 0.1f)
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