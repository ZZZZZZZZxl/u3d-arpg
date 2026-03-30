using GGG.Tool;
using UnityEngine;

public class EnemyApproachState : EnemyState
{
    public EnemyApproachState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        if (_stateMachine.ReusableData.TargetPlayer != null)
            _stateMachine.Enemy.SetDestination(_stateMachine.ReusableData.TargetPlayer.position);
    }

    public override void Update()
    {
        base.Update();

        if (_stateMachine.ReusableData.TargetPlayer == null || AttackDistanceDetection())
            return;

        _stateMachine.Enemy.SetDestination(_stateMachine.ReusableData.TargetPlayer.position);

        if (!UpdateNavigationDirection())
            return;

        Move();
    }

    #endregion


    #region Reusable Methods

    protected override void ChangeApproachState()
    {
    }

    protected override void SetAnimationEnterParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.RunID, true);
    }

    protected override void SetAnimationExitParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.RunID, false);
    }

    #endregion
}
