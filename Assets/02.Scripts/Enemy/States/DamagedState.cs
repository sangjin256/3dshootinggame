using UnityEngine;

public class DamagedState : IState<Enemy>
{
    public void Enter(Enemy enemy)
    {
        enemy.agent.isStopped = true;
        enemy.agent.ResetPath();
    }

    public void Update(Enemy enemy)
    {
        // Damaged state is handled by coroutine
    }

    public void Exit(Enemy enemy)
    {
        enemy.agent.isStopped = false;
    }
} 