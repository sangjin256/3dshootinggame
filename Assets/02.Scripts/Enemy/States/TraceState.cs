using UnityEngine;

public class TraceState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("IdleToMove");
    }

    public void Update(Enemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position);

        if (distanceToPlayer <= enemy.AttackDistance)
        {
            enemy.ChangeState(new AttackState());
            return;
        }

        if (distanceToPlayer > enemy.FindDistance)
        {
            enemy.ChangeState(new ReturnState());
            return;
        }

        enemy.Move(GameManager.Instance.Player.transform.position);
    }

    public void Exit(Enemy enemy)
    {
    }
} 