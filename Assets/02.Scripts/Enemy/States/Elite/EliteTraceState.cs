using UnityEngine;
using UnityEngine.AI;

public class EliteTraceState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("IdleToMove");
    }

    public void Update(Enemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position);
        
        if(distanceToPlayer <= (enemy as EliteEnemy).SpecialAttackDistance)
        {
            enemy.ChangeState(new EliteSpecialAttackState());
            return;
        }
        else if (distanceToPlayer <= enemy.AttackDistance)
        {
            float value = Random.value;
            if (value < 0.8f) enemy.ChangeState(new EliteAttackState());
            else enemy.ChangeState(new EliteSpecialAttackState());
            return;
        }
        else if (distanceToPlayer > enemy.FindDistance)
        {
            enemy.ChangeState(new ElitePatrolState());
            return;
        }
        
        enemy.Move(GameManager.Instance.Player.transform.position);
    }

    public void Exit(Enemy enemy)
    {
    }
} 