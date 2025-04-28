using UnityEngine;

public class FollowAttackState : IState<Enemy>
{
    private float attackElapsedTime;

    public void Enter(Enemy enemy)
    {
        attackElapsedTime = enemy.AttackCooltime;
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, GameManager.I.Player.transform.position) >= enemy.AttackDistance)
        {
            enemy.ChangeState(new FollowTraceState());
            return;
        }

        attackElapsedTime += Time.deltaTime;
        if (attackElapsedTime >= enemy.AttackCooltime)
        {
            Damage damage = new Damage();
            damage.Value = 10;
            damage.From = enemy.gameObject;
            GameManager.I.Player.TakeDamage(damage);
            attackElapsedTime = 0f;
        }
    }

    public void Exit(Enemy enemy)
    {
    }
} 