using GGG.Tool;

public abstract class StateMachine
{
    protected IState _currentState;

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update()
    {
        _currentState?.Update();
    }

    public void HandleInput()
    {
        _currentState?.HandleInput();
    }
    
    public void PhysicsUpdate()
    {
        _currentState?.PhysicsUpdate();
    }

    public void OnAnimationEnter()
    {
        _currentState?.OnAnimationEnter();
    }

    public void OnAnimationExit()
    {
        _currentState?.OnAnimationExit();
    }
    
    public void OnAnimationTransition()
    {
        _currentState?.OnAnimationTransition();
    }

    public void OnAnimationEvent()
    {
        _currentState?.OnAnimationEvent();
    }
    
}