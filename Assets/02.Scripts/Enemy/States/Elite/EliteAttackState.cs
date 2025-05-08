using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EliteAttackState : IState<Enemy>
{
    private float _attackElapsedTime;

    public void Enter(Enemy enemy)
    {
        enemy.GetAnimator().SetTrigger("MoveToAttackDelay");
        _attackElapsedTime = enemy.AttackCooltime;
    }

    public void Update(Enemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position);


        if (distanceToPlayer >= enemy.AttackDistance)
        {
            if (distanceToPlayer <= (enemy as EliteEnemy).SpecialAttackDistance)
            {
                enemy.ChangeState(new EliteSpecialAttackState());
                return;
            }
            else
            {
                enemy.ChangeState(new EliteTraceState());
                return;
            }
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