using UnityEngine;
using UnityEngine.AI;

public class EliteTraceState : IState<Enemy>
{
    private float attackRange = 3f;
    private float specialAttackRange = 5f;
    private float specialAttackCooldown = 10f;
    private float currentCooldown;

    public void Enter(Enemy enemy)
    {
        enemy.SetIsStopped(false);
        enemy.GetAnimator().SetTrigger("IdleToMove");
        enemy.ResetPath();
        currentCooldown = specialAttackCooldown;
    }

    public void Update(Enemy enemy)
    {
        if (GameManager.I.Player == null) return;

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, GameManager.I.Player.transform.position);
        
        if (distanceToPlayer <= enemy.AttackDistance)
        {
            enemy.ChangeState(new EliteAttackState());
        }
        else if (distanceToPlayer > enemy.FindDistance)
        {
            enemy.ChangeState(new ElitePatrolState());
        }
        else
         {
            enemy.Move(GameManager.I.Player.transform.position);
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.ResetPath();
    }
} 