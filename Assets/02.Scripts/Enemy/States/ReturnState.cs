using UnityEngine;

public class ReturnState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.StartPosition) <= enemy.CharacterController.minMoveDistance)
        {
            enemy.transform.position = enemy.StartPosition;
            enemy.ChangeState(new IdleState());
            return;
        }

        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < enemy.FindDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        enemy.agent.SetDestination(enemy.StartPosition);
    }

    public void Exit(Enemy enemy)
    {
    }
} 