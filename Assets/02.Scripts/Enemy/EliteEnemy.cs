using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EliteEnemy : Enemy
{
    public float SpecialAttackDistance = 5f;
    public float SpecialAttackCooltime = 10f;
    public float ExplosionRadius = 5f;
    public float ExplosionDamage = 30f;
    public float SpecialAttackRadius;
    public float SpecialAttackDamage;
    public float KnockbackHeight;
    public float KnockbackDuration;

    public override void TakeDamage(Damage damage)
    {
        if (stateMachine.CurrentState is EliteDieState) return;
        Health -= damage.Value;
        TriggerHitFlash();

        OnHealthChanged?.Invoke();
        
        if(Health <= 0)
        {
            SpawnCoins(100, 150, 30, 40, 12f);
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