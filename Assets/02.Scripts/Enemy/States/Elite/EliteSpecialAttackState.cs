using UnityEngine;
using UnityEngine.AI;

public class EliteSpecialAttackState : IState<Enemy>
{
    private float _specialAttackDuration = 2f;
    private float _elapsedTime = 0f;
    private float _specialAttackDamage = 30f;
    private float _specialAttackRadius = 5f;

    public void Enter(Enemy enemy)
    {
        enemy.SetIsStopped(true);
        enemy.ResetPath();
        _elapsedTime = 0f;
        enemy.GetAnimator()?.SetTrigger("SpecialAttack");
    }

    public void Update(Enemy enemy)
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _specialAttackDuration)
        {
            PerformSpecialAttack(enemy);
            enemy.ChangeState(new EliteTraceState());
        }
    }

    private void PerformSpecialAttack(Enemy enemy)
    {
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, _specialAttackRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Damage damage = new Damage();
                damage.Value = (int)_specialAttackDamage;
                damage.From = enemy.gameObject;
                hitCollider.GetComponent<PlayerController>().TakeDamage(damage);
            }
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.SetIsStopped(false);
    }
} 