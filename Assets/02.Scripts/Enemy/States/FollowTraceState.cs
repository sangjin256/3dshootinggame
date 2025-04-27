using UnityEngine;

public class FollowTraceState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) <= enemy.AttackDistance)
        {
            enemy.ChangeState(new FollowAttackState());
            return;
        }

        enemy.agent.SetDestination(enemy.player.transform.position);
    }

    public void Exit(Enemy enemy)
    {
    }
} 