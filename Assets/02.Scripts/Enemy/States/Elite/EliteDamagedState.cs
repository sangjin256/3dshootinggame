using UnityEngine;

public class EliteDamagedState : IState<Enemy>
{
    private float _elapsedTime = 0f;

    public void Enter(Enemy enemy)
    {
        enemy.SetIsStopped(true);
        _elapsedTime = 0;
        enemy.ResetPath();
        enemy.GetAnimator()?.SetTrigger("Hit");
    }

    public void Update(Enemy enemy)
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= enemy.DamagedTime)
        {
            enemy.ChangeState(new EliteTraceState());
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.SetIsStopped(false);
    }
} 