using UnityEngine;

public class AttackState : IState<Enemy>
{
    private float attackElapsedTime;

    public void Enter(Enemy enemy)
    {
        attackElapsedTime = enemy.AttackCooltime;
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) >= enemy.AttackDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        attackElapsedTime += Time.deltaTime;
        if (attackElapsedTime >= enemy.AttackCooltime)
        {
            Damage damage = new Damage();
            damage.Value = 10;
            damage.From = enemy.gameObject;
            enemy.player.GetComponent<PlayerController>().TakeDamage(damage);
            attackElapsedTime = 0f;
        }
    }

    public void Exit(Enemy enemy)
    {
    }
} 