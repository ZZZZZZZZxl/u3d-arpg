using GGG.Tool;
using UnityEngine.InputSystem;

public class PlayerBeHittingState:PlayerCombatState
{
    private HitInfo _currentHitInfo;
    private Player Player => _stateMachine.Player;
    
    public PlayerBeHittingState(PlayerCombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Meethods

    public override void Enter()
    {
        _currentHitInfo = _stateMachine.ReusableData.HitInfo;
        _stateMachine.ReusableData.ShouldRotate = false;
        base.Enter();
    }

    public override void OnAnimationTransition()
    {
        // if die : chang to die
        
        // 切换到 结束攻击状态 
        _stateMachine.ChangeState(_stateMachine.PlayerHitEndState);
    }

    #endregion


    #region Reusable Methods

    protected override void SetEnterAnimationParameters()
    {
        Player.Animator.SetTrigger(AnimationID.HitID);
        Player.Animator.SetBool(AnimationID.HeavyID, _currentHitInfo.Heavy);
        Player.Animator.SetBool(AnimationID.BeHittingID, true);

        if (DevelopmentToos.IsTargetAtFront(_currentHitInfo.Attacker,
                Player.transform, 180f))
        {
            Player.Animator.SetBool(AnimationID.FrontHitID, true);
        }
        else
        {
            Player.Animator.SetBool(AnimationID.FrontHitID, false);
        }
    }

    protected override void SetExitAnimationParameters()
    {
        Player.Animator.SetBool(AnimationID.HeavyID, false);
        Player.Animator.SetBool(AnimationID.FrontHitID, false);
        Player.Animator.SetBool(AnimationID.BeHittingID, false);
    }
    
    // 禁用死亡逻辑

    #endregion


    #region InputActions

    protected override void OnFinalAttackStarted(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnSpecialAttackStarted(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
        
    }

    #endregion
    
}