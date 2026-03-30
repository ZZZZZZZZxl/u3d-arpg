
public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
    public void HandleInput();
    public void PhysicsUpdate();
    public void OnAnimationEnter();
    public void OnAnimationExit();
    public void OnAnimationTransition();

    public void OnAnimationEvent();

}
