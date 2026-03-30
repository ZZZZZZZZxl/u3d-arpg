/*
 * Can Reach:
 *  RunningEnd, Dashing, Sprinting
 *  RunningEnd: IntputAction movement cancled
 *  Dashing: (All States) Dash started ? 
 */

using GGG.Tool;
using UnityEngine;

public class PlayerRunningState : PlayerGroundedState
{
    private float _startTime;
    public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        
        base.Enter();

        _stateMachine.ReusableData.ShouldRotate = true;
        _stateMachine.ReusableData.MovementSpeedModifier = _movementData.PlayerRunData.SpeedModifier;
        _startTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (Time.time < _startTime + _movementData.PlayerRunData.RunToSprintTime)
            return;
        
        _stateMachine.ChangeState(_stateMachine.PlayerSprintingState);
    }
    
    public override void OnAnimationTransition()
    {
        DevelopmentToos.WTF("发生Transition的是: Running!!");
    }

    public override void Exit()
    {
        base.Exit();
        
        _stateMachine.Player.Animator.SetBool(AnimationID.IsMovingID, false);
        _stateMachine.Player.Animator.SetBool(AnimationID.HasInputId, false);
    }

    #endregion


    #region Reusable Methods

    protected override void SetAnimatorParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.IsMovingID, true);
        _stateMachine.Player.Animator.SetBool(AnimationID.HasInputId, true);
    }

    #endregion
    
}