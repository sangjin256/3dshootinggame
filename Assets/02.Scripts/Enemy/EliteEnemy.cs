using UnityEngine;

public class EliteEnemy : Enemy
{
    [Header("Elite Enemy Settings")]
    public float specialAttackRange = 5f;
    public float specialAttackCooldown = 10f;
    public float explosionRadius = 5f;
    public float explosionDamage = 30f;

    public override void TakeDamage(Damage damage)
    {
        if (stateMachine.CurrentState is EliteDieState) return;
        Health -= damage.Value;
        TriggerHitFlash();

        OnHealthChanged?.Invoke();
        
        if(Health <= 0)
        {
            ChangeState(new EliteDieState());
            return;
        }

        ChangeState(new EliteDamagedState());
    }

    protected override void InitializeEnemy()
    {
        base.InitializeEnemy();
        ChangeState(new EliteTraceState());
        // 엘리트 몬스터 특화 초기화
        _agent.speed = MoveSpeed * 1.5f; // 더 빠른 이동 속도
    }

    public override Vector3 GetPatrolPosition() { return Vector3.zero; }
    public override int GetPatrolPositionsCount() { return 0; }
} 