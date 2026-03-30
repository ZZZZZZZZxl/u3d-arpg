using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState:PlayerGroundedState
{
    #region IState Methods

    public PlayerDashingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _stateMachine.Player.transform.rotation = Quaternion.Euler(0, _stateMachine.ReusableData.CurrentTargetRotation, 0);
        _stateMachine.ReusableData.ShouldRotate = false;
    }

    #endregion
    
    
    #region InputActions

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        _stateMachine.SetToRushAttackingState();
    }

    #endregion
}