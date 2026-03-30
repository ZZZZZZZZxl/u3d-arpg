using System;
using GGG.Tool;
using UnityEngine.InputSystem;

public class PlayerFinalAttackingState : PlayerAttackingState
{
    private float _canInputTimeRate;

    //采取谁最后输入转换谁的策略 0 没有输入 1 普通攻击 2 特殊攻击
    private int _currentInputType;

    public PlayerFinalAttackingState(PlayerCombatStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        
        _stateMachine.ReusableData.ShouldRotate = false;
    }

    public override void Exit()
    {
        base.Exit();

        _currentInputType = 0;
    }

    public override void OnAnimationTransition()
    {
        if (_currentInputType == 0)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerFinalAttackEndState);
            return;
        }

        if (_currentInputType == 1)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerNormalAttackingState);
            return;
        }

        if (_currentInputType == 2)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerSpecialAttackingState);
            return;
        }
    }

    #endregion
    

    #region Reusable Methods
    
    protected override void SetEnterAnimationParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.FinalAttackID, true);
    }

    protected override void SetExitAnimationParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.FinalAttackID, false);
    }

    #endregion


    #region Main Methods

    private bool CheckAnimation()
    {
        if (_stateMachine.Player.Animator.IsInTransition(0)) 
            return false;
        if (!_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).IsName("FinalAttack"))
            return false;
        return true;
    }

    #endregion
    
    
    #region InputActions

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (!CheckAnimation()) 
            return;
        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < _canInputTimeRate)
            return;
        
        _currentInputType = 1;
    }

    protected override void OnSpecialAttackStarted(InputAction.CallbackContext context)
    {
        if (!CheckAnimation()) 
            return;
        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < _canInputTimeRate)
            return;
        
        _currentInputType = 2;
    }

    protected override void OnFinalAttackStarted(InputAction.CallbackContext context)
    {
        
    }

    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
        
    }

    #endregion
}