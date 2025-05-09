using UnityEngine;

public class FollowAttackState : IState<Enemy>
{
    private float attackElapsedTime;

    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("MoveToAttackDelay");
        attackElapsedTime = enemy.AttackCooltime;
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position) >= enemy.AttackDistance)
        {
            enemy.ChangeState(new FollowTraceState());
            return;
        }

        attackElapsedTime += Time.deltaTime;
        if (attackElapsedTime >= enemy.AttackCooltime)
        {
            enemy.GetAnimator().SetTrigger("AttackDelayToAttack");
            Damage damage = new Damage();
            damage.Value = 10;
            damage.From = enemy.gameObject;
            GameManager.Instance.Player.TakeDamage(damage);
            attackElapsedTime = 0f;
        }
    }

    public void Exit(Enemy enemy)
    {
    }
} 