using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingBackState : PlayerDashingState
{
    public PlayerDashingBackState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        
        _stateMachine.ReusableData.MovementSpeedModifier = 0f;
    }

    public override void PhysicsUpdate()
    {
        
    }
    
    public override void Exit()
    {
        base.Exit();
        
        _stateMachine.Player.Animator.SetBool(AnimationID.EndDashID, true);
    }
    
    public override void OnAnimationTransition()
    {
        if (_stateMachine.ReusableData.MovementInput != Vector2.zero)
            _stateMachine.ChangeState(_stateMachine.PlayerStartRunningState);
        else _stateMachine.ChangeState(_stateMachine.PlayerIdlingState);
    }
    
    #endregion


    #region Reusable Methods

    protected override void SetAnimatorParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.EndDashID, false);
    }

    #endregion
    
}