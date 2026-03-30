using GGG.Tool;
using UnityEngine;

public class EnemyHitState : EnemyState
{
    private HitInfo _currentHitInfo;

    public EnemyHitState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        _currentHitInfo = _stateMachine.ReusableData.HitInfo;
        StopNavigation(true);
        base.Enter();
    }

    public override void OnAnimationTransition()
    {
        if (_stateMachine.Enemy.HealthController.IsDead)
        {
            _stateMachine.ChangeState(_stateMachine.DeadState);
            return;
        }

        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    #endregion


    #region Reusable Methods

    protected override void SetAnimationEnterParameters()
    {
        Enemy.Animator.SetTrigger(AnimationID.HitID);
        Enemy.Animator.SetBool(AnimationID.HeavyID, _currentHitInfo.Heavy);
        Enemy.Animator.SetBool(AnimationID.BeHittingID, true);

        if (DevelopmentToos.IsTargetAtFront(_currentHitInfo.Attacker, _stateMachine.Enemy.transform, 200f))
        {
            Enemy.Animator.SetBool(AnimationID.FrontHitID, true);
            _stateMachine.Enemy.transform.Look(_currentHitInfo.Attacker.position, 5000f);
            return;
        }

        Enemy.Animator.SetBool(AnimationID.FrontHitID, false);
    }

    protected override void SetAnimationExitParameters()
    {
        Enemy.Animator.SetBool(AnimationID.HeavyID, false);
        Enemy.Animator.SetBool(AnimationID.FrontHitID, false);
        Enemy.Animator.SetBool(AnimationID.BeHittingID, false);
    }

    protected override void ChangeDieState()
    {
    }

    #endregion
}
