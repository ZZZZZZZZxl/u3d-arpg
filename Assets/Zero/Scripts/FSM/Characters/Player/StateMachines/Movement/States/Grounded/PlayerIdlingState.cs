using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
    public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
  
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.ReusableData.ShouldRotate = true;
        _stateMachine.ReusableData.MovementSpeedModifier = 0f;
    }

    public override void Update()
    {
        base.Update();
        
        if (_stateMachine.ReusableData.MovementInput != Vector2.zero)
            OnMove(); // Unmoving To Moving
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        _stateMachine.Player.Animator.SetBool(AnimationID.EndToIdleID, false);
    }

    #endregion

    #region Reusable Methods

    protected override void SetAnimatorParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.HasInputId, false);
        _stateMachine.Player.Animator.SetBool(AnimationID.EndToIdleID, true);
    }

    #endregion
    
}