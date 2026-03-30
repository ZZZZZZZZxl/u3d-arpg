using UnityEngine;

public class PlayerCombatStateMachine : StateMachine
{
    private PlayerFinalAttackingState _playerFinalAttackingState;
    private PlayerNormalAttackingState _playerNormalAttackingState;
    private PlayerSpecialAttackingState _playerSpecialAttackingState;
    private PlayerNormalAttackEndState _playerPlayerNormalAttackEndState;
    private PlayerRushAttackingState _playerRushAttackingState;
    private PlayerFinalAttackEndState _playerFinalAttackEndState;
    private PlayerRushAttackingEndState _playerRushAttackingEndState;
    private PlayerBeHittingState _playerBeHittingState;
    private PlayerHitEndState _playerHitEndState;
    
    private Player _player;
    
    public PlayerFinalAttackingState PlayerFinalAttackingState => _playerFinalAttackingState;
    public PlayerNormalAttackingState PlayerNormalAttackingState => _playerNormalAttackingState;
    public PlayerSpecialAttackingState PlayerSpecialAttackingState => _playerSpecialAttackingState;
    public PlayerNormalAttackEndState PlayerNormalAttackEndState => _playerPlayerNormalAttackEndState;
    public PlayerRushAttackingState PlayerRushAttackingState => _playerRushAttackingState;
    public PlayerFinalAttackEndState PlayerFinalAttackEndState => _playerFinalAttackEndState;
    public PlayerRushAttackingEndState PlayerRushAttackingEndState => _playerRushAttackingEndState;
    public PlayerBeHittingState PlayerBeHittingState => _playerBeHittingState;
    public PlayerHitEndState PlayerHitEndState => _playerHitEndState;
    
    public Player Player => _player;

    private PlayerStateReusableData _reusableData;
    public PlayerStateReusableData ReusableData => _reusableData;

    public PlayerCombatStateMachine(Player player)
    {
        _player = player;

        _reusableData = Player.ReusableData;
        
        _playerFinalAttackingState = new PlayerFinalAttackingState(this);
        _playerNormalAttackingState = new PlayerNormalAttackingState(this);
        _playerSpecialAttackingState = new PlayerSpecialAttackingState(this);
        _playerPlayerNormalAttackEndState = new PlayerNormalAttackEndState(this);
        _playerRushAttackingState = new PlayerRushAttackingState(this);
        _playerFinalAttackEndState = new PlayerFinalAttackEndState(this);
        _playerRushAttackingEndState = new PlayerRushAttackingEndState(this);
        _playerBeHittingState = new PlayerBeHittingState(this);
        _playerHitEndState = new PlayerHitEndState(this);
    }

    // 回到默认状态
    public void SetToIdleState()
    {
        ExitCurrentState();
        _player.MovementStateMachine.ChangeState(_player.MovementStateMachine.PlayerIdlingState);
    }

    public void SetToDashingState()
    {
        ExitCurrentState();
        if (_reusableData.MovementInput != Vector2.zero)
            _player.MovementStateMachine.ChangeState(_player.MovementStateMachine.PlayerDashingForwardState);
        else _player.MovementStateMachine.ChangeState(_player.MovementStateMachine.PlayerDashingBackState);
    }

    public void SetToStartRunningState()
    {
        ExitCurrentState();
        _player.MovementStateMachine.ChangeState(_player.MovementStateMachine.PlayerStartRunningState);
    }

    public void ExitCurrentState()
    {
        ChangeState(null);
    }
}