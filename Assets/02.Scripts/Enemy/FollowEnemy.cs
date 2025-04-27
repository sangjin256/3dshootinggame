using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FollowEnemy : Enemy
{
    protected override void InitializeEnemy()
    {
        base.InitializeEnemy();
        stateMachine.ChangeState(new FollowTraceState());
    }

    protected override void Update()
    {
        stateMachine.Update();
    }

    public override void TakeDamage(Damage damage)
    {
        if (stateMachine.CurrentState is DamagedState) return;

        Health -= damage.Value;
        OnHealthChanged?.Invoke();
        
        if(Health <= 0)
        {
            StartCoroutine(Die_Coroutine());
            return;
        }

        ChangeState(new DamagedState());
        StartCoroutine(Damagaed_Coroutine());
    }

    private IEnumerator Damagaed_Coroutine()
    {
        agent.isStopped = true;
        agent.ResetPath();
        yield return new WaitForSeconds(DamagedTime);
        ChangeState(new FollowTraceState());
    }

    public override Vector3 GetPatrolPosition()
    {
        return Vector3.zero;
    }

    public override int GetPatrolPositionsCount()
    {
        return 0;
    }
}
