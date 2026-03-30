using GGG.Tool;
using UnityEngine;

public class EnemyReturnState : EnemyState
{
    public EnemyReturnState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        _stateMachine.Enemy.SetDestination(_stateMachine.ReusableData.OriginPosition);
    }

    public override void Update()
    {
        base.Update();

        if (_stateMachine.ReusableData.TargetPlayer != null)
            return;

        _stateMachine.Enemy.SetDestination(_stateMachine.ReusableData.OriginPosition);

        if (!UpdateNavigationDirection())
        {
            if (_stateMachine.Enemy.HasReachedDestination())
                _stateMachine.ChangeState(_stateMachine.IdleState);

            return;
        }

        Move();

        if (_stateMachine.Enemy.HasReachedDestination())
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    #endregion


    #region Reusable Methods

    protected override void SetAnimationEnterParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.WalkID, true);
    }

    protected override void SetAnimationExitParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.WalkID, false);
    }

    #endregion
}
