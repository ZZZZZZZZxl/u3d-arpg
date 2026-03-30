public class PlayerRushAttackingEndState:PlayerAttackEndState
{
    public PlayerRushAttackingEndState(PlayerCombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.ReusableData.MovementSpeedModifier =
            _stateMachine.Player.Data.GroundedData.PlayerSprintData.SpeedModifier;
    }

    #endregion
}