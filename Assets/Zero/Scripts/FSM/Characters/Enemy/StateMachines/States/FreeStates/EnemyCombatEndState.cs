using GGG.Tool;

public class EnemyCombatEndState : EnemyState
{
 

    public EnemyCombatEndState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        
        // DevelopmentToos.WTF("敌人在收刀");
    }

    public override void OnAnimationTransition()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    #endregion


    #region Reusable Methods
    

    #endregion
}