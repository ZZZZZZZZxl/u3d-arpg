/*
 *  Can Reach :
 *  TrunAround: deltaAngle > ?f
 *  Dashing: (All States) Dash started ?
 *  RunningEnd: IntputAction movement cancled
 */

using GGG.Tool;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerGroundedState
{
    public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.ReusableData.ShouldRotate = true;
        _stateMachine.ReusableData.MovementSpeedModifier = _movementData.PlayerSprintData.SpeedModifier;
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


    #region InputActions

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        _stateMachine.SetToRushAttackingState();
    }

    #endregion
    
}