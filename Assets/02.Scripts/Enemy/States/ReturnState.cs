using UnityEngine;

public class ReturnState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.GetStartPosition()) <= enemy.GetMinDistance())
        {
            enemy.transform.position = enemy.GetStartPosition();
            enemy.ChangeState(new IdleState());
            return;
        }

        if (Vector3.Distance(enemy.transform.position, GameManager.I.Player.transform.position) < enemy.FindDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        enemy.Move(enemy.GetStartPosition());
    }

    public void Exit(Enemy enemy)
    {
    }
} 