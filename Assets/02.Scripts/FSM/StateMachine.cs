using System;

public class StateMachine<T>
{
    private T owner;
    private IState<T> currentState;
    private IState<T> previousState;

    public IState<T> CurrentState => currentState;

    public StateMachine(T owner)
    {
        this.owner = owner;
    }

    public void ChangeState(IState<T> newState)
    {
        if (currentState != null)
        {
            currentState.Exit(owner);
        }

        previousState = currentState;
        currentState = newState;
        currentState.Enter(owner);
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update(owner);
        }
    }
} 