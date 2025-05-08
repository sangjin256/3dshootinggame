using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EliteSpecialAttackState : IState<Enemy>
{
    private float _attackElapsedTime;
    private EliteEnemy _eliteEnemy;

    public void Enter(Enemy enemy)
    {
        _eliteEnemy = enemy as EliteEnemy;
        enemy.GetAnimator().SetTrigger("MoveToAttackDelay");
        _attackElapsedTime = 0;
    }

    public void Update(Enemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position);

        if (distanceToPlayer >= _eliteEnemy.SpecialAttackDistance)
        {
            enemy.ChangeState(new EliteTraceState());
            return;
        }

        _attackElapsedTime += Time.deltaTime;
        if (_attackElapsedTime >= _eliteEnemy.SpecialAttackCooltime)
        {
            enemy.GetAnimator().SetTrigger("AttackDelayToSpecialAttack");
            PerformSpecialAttack(enemy);
            _attackElapsedTime = 0f;

            if (distanceToPlayer <= _eliteEnemy.SpecialAttackDistance)
            {
                float value = Random.value;
                if (value < 0.6f)
                {
                    enemy.ChangeState(new EliteAttackState());
                    return;
                }
            }
        }
    }

    private void PerformSpecialAttack(Enemy enemy)
    {
        Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, (enemy as EliteEnemy).SpecialAttackRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Damage damage = new Damage();
                damage.Value = (int)(enemy as EliteEnemy).SpecialAttackDamage;
                damage.From = enemy.gameObject;
                GameManager.Instance.Player.TakeDamage(damage);
                ApplyKnockback(GameManager.Instance.Player.GetComponent<CharacterController>(), enemy);
            }
        }
    }

    private void ApplyKnockback(CharacterController controller, Enemy enemy)
    {
        if (controller == null) return;

        // 넉백 코루틴 시작
        enemy.StartCoroutine(KnockbackRoutine(controller));
    }

    private IEnumerator KnockbackRoutine(CharacterController controller)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = controller.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * _eliteEnemy.KnockbackHeight;

        while (elapsedTime < _eliteEnemy.KnockbackDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _eliteEnemy.KnockbackDuration;

            float height = Mathf.Sin(t * Mathf.PI) * _eliteEnemy.KnockbackDuration;
            Vector3 newPosition = startPosition + Vector3.up * height;

            controller.Move(newPosition - controller.transform.position);

            yield return null;
        }

        controller.Move(startPosition - controller.transform.position);
    }

    public void Exit(Enemy enemy)
    {
        enemy.SetIsStopped(false);
    }
} 