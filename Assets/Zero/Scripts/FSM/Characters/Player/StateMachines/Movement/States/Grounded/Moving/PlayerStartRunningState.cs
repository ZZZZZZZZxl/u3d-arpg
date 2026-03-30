using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStartRunningState : PlayerGroundedState
{
    private float _startTime;
    public PlayerStartRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }
    
    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        _stateMachine.ReusableData.ShouldRotate = true;

        if (_stateMachine.ReusableData.MovementSpeedModifier == 0f)
        {
            _stateMachine.ReusableData.MovementSpeedModifier = _movementData.PlayerRunData.SpeedModifier;
        }
        
        _startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        
        _stateMachine.Player.Animator.SetBool(AnimationID.HasInputId, false);
    }

    public override void OnAnimationTransition()
    {
        if (Mathf.Approximately(_stateMachine.ReusableData.MovementSpeedModifier, _movementData.PlayerRunData.SpeedModifier))
            _stateMachine.ChangeState(_stateMachine.PlayerRunningState);
        else _stateMachine.ChangeState(_stateMachine.PlayerSprintingState);
    }

    #endregion


    #region Reusable Methods

    protected override void SetAnimatorParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.HasInputId, true);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (Time.time < _startTime + _movementData.PlayerStartRunningData.TimeToIdle)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerIdlingState);
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.PlayerStopRunningState);
        }
    }

    #endregion


    #region InputActions

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (Mathf.Approximately(_stateMachine.ReusableData.MovementSpeedModifier, _movementData.PlayerSprintData.SpeedModifier))
        {
            _stateMachine.SetToRushAttackingState();
            return;
        }
        _stateMachine.SetToNormalAttackState();
    }

    #endregion
    
}