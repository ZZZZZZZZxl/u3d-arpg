/*
dashing
- -> idling
- -> spriting
*/

using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingForwardState : PlayerDashingState
{
    public PlayerDashingForwardState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }
    
    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.ReusableData.MovementSpeedModifier = _movementData.PlayerSprintData.SpeedModifier;
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnAnimationTransition()
    {
        if (_stateMachine.ReusableData.MovementInput != Vector2.zero)
            _stateMachine.ChangeState(_stateMachine.PlayerStartRunningState);
        else _stateMachine.ChangeState(_stateMachine.PlayerIdlingState);
    }

    public override void Exit()
    {
        base.Exit();
        
        _stateMachine.Player.Animator.SetBool(AnimationID.EndDashID, true);
        _stateMachine.Player.Animator.SetBool(AnimationID.HasInputId, false);
    }

    #endregion
    
    
    #region Reusable Methods

    protected override void SetAnimatorParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.EndDashID, false);
        _stateMachine.Player.Animator.SetBool(AnimationID.HasInputId, true);
    }

    #endregion
    
}