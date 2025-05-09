using UnityEngine;

public class IdleState : IState<Enemy>
{
    private float idleElapsedTime = 0f;

    public void Enter(Enemy enemy)
    {
        idleElapsedTime = 0f;
        enemy.GetAnimator().SetTrigger("MoveToIdle");
    }

    public void Update(Enemy enemy)
    {
        if (Vector3.Distance(enemy.transform.position, GameManager.Instance.Player.transform.position) < enemy.FindDistance)
        {
            enemy.ChangeState(new TraceState());
            return;
        }

        idleElapsedTime += Time.deltaTime;
        if (idleElapsedTime >= enemy.IdleCoolTime)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit(Enemy enemy)
    {
    }
}