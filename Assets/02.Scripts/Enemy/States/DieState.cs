using UnityEngine;

public class DieState : IState<Enemy>
{
    private float _elapsedTime = 0;
    public void Enter(Enemy enemy)
    {
        enemy.ResetPath();
        _elapsedTime = 0f;
        enemy.GetAnimator().SetTrigger("Die");
    }

    public void Update(Enemy enemy)
    {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= enemy.DeathTime)
        {
            enemy.ReturnToPool();
        }
    }

    public void Exit(Enemy enemy)
    {

    }
}
