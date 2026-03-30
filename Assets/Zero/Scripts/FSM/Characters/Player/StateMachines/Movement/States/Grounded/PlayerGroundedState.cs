using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
    { }

    #region IState Methods

    public override void Enter()
    {
        AddInputActionsCallbacks();
        base.Enter();
    }

    public override void Exit() 
    {
        RemoveInputActionsCallbacks();
        base.Exit();
    }

    #endregion
    
    #region Reusable Methods

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInputManager.MainInstance.PlayerActions.Movement.canceled += OnMovementCanceled;
        PlayerInputManager.MainInstance.PlayerActions.Dash.started += OnDashStarted;
        PlayerInputManager.MainInstance.PlayerActions.Attack.started += OnAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Special.started += OnSpecialAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Final.started += OnFinalAttackStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInputManager.MainInstance.PlayerActions.Movement.canceled -= OnMovementCanceled;
        PlayerInputManager.MainInstance.PlayerActions.Dash.started -= OnDashStarted;
        PlayerInputManager.MainInstance.PlayerActions.Attack.started -= OnAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Special.started -= OnSpecialAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Final.started -= OnFinalAttackStarted;
    }
    
    protected virtual void OnMove()
    {
        _stateMachine.ChangeState(_stateMachine.PlayerStartRunningState);
    }

    #endregion

    #region InputActions

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerStopRunningState);
    }

    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        _stateMachine.Player.Animator.SetTrigger(AnimationID.DashID);
        if (_stateMachine.ReusableData.MovementInput != Vector2.zero)
            _stateMachine.ChangeState(_stateMachine.PlayerDashingForwardState);
        else _stateMachine.ChangeState(_stateMachine.PlayerDashingBackState);
    }

    protected virtual void OnFinalAttackStarted(InputAction.CallbackContext context)
    {
        _stateMachine.SetToFinalAttackState();
    }

    protected virtual void OnSpecialAttackStarted(InputAction.CallbackContext context)
    {
        _stateMachine.SetToSpecialAttackState();
    }

    protected virtual void OnAttackStarted(InputAction.CallbackContext context)
    {
        _stateMachine.SetToNormalAttackState();
    }

    #endregion
}