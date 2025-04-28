using UnityEngine;

public class TraceState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("IdleToMove");
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, GameManager.I.Player.transform.position) <= enemy.AttackDistance)
        {
            enemy.ChangeState(new AttackState());
            return;
        }

        if (Vector3.Distance(enemy.transform.position, GameManager.I.Player.transform.position) >= enemy.ReturnDistance)
        {
            enemy.ChangeState(new ReturnState());
            return;
        }

        enemy.Move(GameManager.I.Player.transform.position);
    }

    public void Exit(Enemy enemy)
    {
    }
} 