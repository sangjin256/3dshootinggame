using UnityEngine;

public class FollowDamagedState : IState<Enemy>
{
    private float _elapsedTime = 0f;

    public void Enter(Enemy enemy)
    {
        enemy.SetIsStopped(true);
        _elapsedTime = 0;
        enemy.ResetPath();
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
        enemy.SetIsStopped(false);
    }
}
