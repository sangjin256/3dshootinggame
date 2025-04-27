using UnityEngine;

public class TraceState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) <= enemy.AttackDistance)
        {
            enemy.ChangeState(new AttackState());
            return;
        }

        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) >= enemy.ReturnDistance)
        {
            enemy.ChangeState(new ReturnState());
            return;
        }

        enemy.agent.SetDestination(enemy.player.transform.position);
    }

    public void Exit(Enemy enemy)
    {
    }
} 