public class PlayerTurningAroundState : PlayerGroundedState
{
    public PlayerTurningAroundState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }
    
    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.ReusableData.ShouldRotate = false;
        _stateMachine.ReusableData.MovementSpeedModifier = _movementData.PlayerSprintData.SpeedModifier;
    }
    
    

    #endregion
}