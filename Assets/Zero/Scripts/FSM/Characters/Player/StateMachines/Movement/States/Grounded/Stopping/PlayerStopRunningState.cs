using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStopRunningState : PlayerGroundedState
{
    public PlayerStopRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    { }
    
    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        _stateMachine.ReusableData.ShouldRotate = false;

        _stateMachine.ReusableData.MovementSpeedModifier = 0f;
    }


    public override void Update()
    {
        base.Update();

        if (_stateMachine.ReusableData.MovementInput == Vector2.zero) return;
        
        OnMove();
    }

    public override void OnAnimationTransition()
    {
        _stateMachine.ChangeState(_stateMachine.PlayerIdlingState);
    }

    #endregion
    

    #region Resuable Methods

    protected override void SetAnimatorParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.HasInputId, false);
    }

    #endregion
    
    
    #region InputActions

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        
    }

    #endregion
}