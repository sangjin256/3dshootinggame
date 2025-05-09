using UnityEngine;

public class AttackState : IState<Enemy>
{
    private float _attackElapsedTime;

    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("MoveToAttackDelay");
        _attackElapsedTime = enemy.AttackCooltime;
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position) >= enemy.AttackDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        _attackElapsedTime += Time.deltaTime;
        if (_attackElapsedTime >= enemy.AttackCooltime)
        {
            enemy.GetAnimator().SetTrigger("AttackDelayToAttack");
            Damage damage = new Damage();
            damage.Value = 10;
            damage.From = enemy.gameObject;
            GameManager.Instance.Player.TakeDamage(damage);
            _attackElapsedTime = 0f;
        }
    }

    public void Exit(Enemy enemy)
    {
    }
} 