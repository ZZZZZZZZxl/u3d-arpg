using GGG.Tool;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyApproachState:EnemyState
{
    
    public EnemyApproachState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
    }

    #endregion


    #region Main Methods

    

    #endregion
    
    
    #region Reusable Methods

    protected override void ChangeApproachState()
    {
        
    }

    protected override void SetAnimationEnterParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.RunID, true);
    }

    override protected void SetAnimationExitParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.RunID, false);
    }
    #endregion
}