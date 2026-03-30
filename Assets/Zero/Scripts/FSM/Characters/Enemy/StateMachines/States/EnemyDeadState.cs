public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        DisableEnemyController();
    }

    #endregion


    #region Main Methods

    private void DisableEnemyController()
    {
        Enemy.Controller.enabled = false;
    }

    #endregion


    #region Reusable Methods

    protected override void SetAnimationEnterParameters()
    {
        Enemy.Animator.SetBool(AnimationID.DieID, true);
    }

    protected override void ChangeHitState()
    {
        
    }

    protected override void ChangeDieState()
    {
        
    }

    #endregion
}
