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

        _eliteEnemy.OnSpecialAttack = PerformSpecialAttack;
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

    private void PerformSpecialAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_eliteEnemy.transform.position, _eliteEnemy.SpecialAttackRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Damage damage = new Damage();
                damage.Value = (int)_eliteEnemy.SpecialAttackDamage;
                damage.From = _eliteEnemy.gameObject;
                GameManager.Instance.Player.TakeDamage(damage);
                ApplyKnockback(GameManager.Instance.Player.GetComponent<CharacterController>(), _eliteEnemy);
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