using UnityEngine;

public class BasicEnemy : Enemy
{
    [SerializeField] private Vector3[] PatrolPositionArr = { 
        new Vector3(-11, 1.08f, 2), 
        new Vector3(-11, 1.08f, 18), 
        new Vector3(6, 1.08f, 18), 
        new Vector3(6, 1.08f, 2)
    };
    protected override void InitializeEnemy()
    {
        base.InitializeEnemy();
        stateMachine.ChangeState(new IdleState());
    }

    public override Vector3 GetPatrolPosition()
    {
        return PatrolPositionArr[CurrentPatrolIndex];
    }

    public override int GetPatrolPositionsCount()
    {
        return PatrolPositionArr.Length;
    }
}
