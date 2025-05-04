using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EliteAttackState : IState<Enemy>
{
    private float _attackCooldown = 0f;
    private float _attackRange = 2f;
    private float _attackDamage = 20f;
    private float attackRadius = 2f;
    private float knockbackForce = 5f;
    private float knockbackDuration = 0.5f;
    private float knockbackHeight = 2f;

    public void Enter(Enemy enemy)
    {
        enemy.SetIsStopped(true);
        enemy.ResetPath();
        enemy.GetAnimator()?.SetTrigger("Attack");
    }

    public void Update(Enemy enemy)
    {
        _attackCooldown -= Time.deltaTime;

        if (_attackCooldown <= 0f)
        {
            PerformAttack(enemy);
            _attackCooldown = 2f; // 공격 쿨다운
        }

        // 플레이어와의 거리 확인
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, GameManager.I.Player.transform.position);
        if (distanceToPlayer > _attackRange)
        {
            enemy.ChangeState(new EliteTraceState());
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.SetIsStopped(false);
    }

    private void PerformAttack(Enemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, GameManager.I.Player.transform.position);
        if (distanceToPlayer <= _attackRange)
        {
            Damage damage = new Damage();
            damage.Value = (int)_attackDamage;
            damage.From = enemy.gameObject;
            GameManager.I.Player.TakeDamage(damage);
            enemy.GetAnimator()?.SetTrigger("Attack");
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
        Vector3 targetPosition = startPosition + Vector3.up * knockbackHeight;

        while (elapsedTime < knockbackDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / knockbackDuration;

            // 부드러운 상승과 하강을 위한 사인 곡선 사용
            float height = Mathf.Sin(t * Mathf.PI) * knockbackHeight;
            Vector3 newPosition = startPosition + Vector3.up * height;

            // CharacterController로 이동
            controller.Move(newPosition - controller.transform.position);

            yield return null;
        }

        // 최종 위치 보정
        controller.Move(startPosition - controller.transform.position);
    }
} 