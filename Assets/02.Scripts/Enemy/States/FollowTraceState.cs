using UnityEngine;

public class FollowTraceState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("IdleToMove");
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position) <= enemy.AttackDistance)
        {
            enemy.ChangeState(new FollowAttackState());
            return;
        }

        enemy.Move(GameManager.Instance.Player.transform.position);
    }

    public void Exit(Enemy enemy)
    {
    }
} 