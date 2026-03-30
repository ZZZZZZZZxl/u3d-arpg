public class EnemyCombatEndState : EnemyState
{
    public EnemyCombatEndState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        StopNavigation(true);
        base.Enter();
    }

    public override void OnAnimationTransition()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    #endregion
}
