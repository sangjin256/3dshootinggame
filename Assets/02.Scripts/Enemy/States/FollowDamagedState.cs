using UnityEngine;

public class FollowDamagedState : IState<Enemy>
{
    private float _elapsedTime = 0f;

    public void Enter(Enemy enemy)
    {
        enemy.agent.isStopped = true;
        _elapsedTime = 0;
        enemy.agent.ResetPath();
    }

    public void Update(Enemy enemy)
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= enemy.DamagedTime)
        {
            enemy.ChangeState(new FollowTraceState());
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.agent.isStopped = false;
    }
}
