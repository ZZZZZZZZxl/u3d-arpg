using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    private PlayerIdlingState _playerIdlingState;
    private PlayerStartRunningState _playerStartRunningState;
    private PlayerDashingForwardState _playerDashingForwardState;
    private PlayerDashingBackState _playerDashingBackState;
    
    
    private PlayerRunningState _playerRunningState;
    private PlayerSprintingState _playerSprintingState;
    
    
    private PlayerStopRunningState _playerStopRunningState;
    private PlayerTurningAroundState _playerTurningAroundState;
    
    
    public PlayerIdlingState PlayerIdlingState => _playerIdlingState;
    public PlayerStartRunningState PlayerStartRunningState => _playerStartRunningState;
    public PlayerDashingForwardState PlayerDashingForwardState => _playerDashingForwardState;
    public PlayerDashingBackState PlayerDashingBackState => _playerDashingBackState;
    public PlayerRunningState PlayerRunningState => _playerRunningState;
    public PlayerSprintingState PlayerSprintingState => _playerSprintingState;
    public PlayerStopRunningState PlayerStopRunningState => _playerStopRunningState;
    public PlayerTurningAroundState PlayerTurningAroundState => _playerTurningAroundState;


    private Player _player;
    public Player Player => _player;


    private PlayerStateReusableData _reusableData;
    public  PlayerStateReusableData ReusableData => _reusableData;
    
    
    public PlayerMovementStateMachine(Player player)
    {
        _player = player;
        _reusableData = _player.ReusableData;
        
        _playerIdlingState = new PlayerIdlingState(this);
        _playerStartRunningState = new PlayerStartRunningState(this);
        _playerDashingForwardState = new PlayerDashingForwardState(this);
        _playerDashingBackState = new PlayerDashingBackState(this);
        _playerRunningState = new PlayerRunningState(this);
        _playerSprintingState = new PlayerSprintingState(this);
        _playerStopRunningState = new PlayerStopRunningState(this);
        _playerTurningAroundState = new PlayerTurningAroundState(this);
    }

    public void SetToNormalAttackState()
    {
        ExitMovementState();
        _player.CombatStateMachine.ChangeState(_player.CombatStateMachine.PlayerNormalAttackingState);
    }

    public void SetToRushAttackingState()
    {
        ExitMovementState();
        _player.CombatStateMachine.ChangeState(_player.CombatStateMachine.PlayerRushAttackingState);
    }
    
    public void SetToSpecialAttackState()
    {
        ExitMovementState();
        _player.CombatStateMachine.ChangeState(_player.CombatStateMachine.PlayerSpecialAttackingState);
    }
    
    public void SetToFinalAttackState()
    {
        ExitMovementState();
        _player.CombatStateMachine.ChangeState(_player.CombatStateMachine.PlayerFinalAttackingState);
    }

    public void SetToHitState()
    {
        ExitMovementState();
        _player.CombatStateMachine.ChangeState(_player.CombatStateMachine.PlayerBeHittingState);
    }

    private void ExitMovementState()
    {
        ChangeState(null);
        _reusableData.ResetData();
        // _player.Animator.SetFloat(AnimationID.MovementId, 0f);
    }
    
}