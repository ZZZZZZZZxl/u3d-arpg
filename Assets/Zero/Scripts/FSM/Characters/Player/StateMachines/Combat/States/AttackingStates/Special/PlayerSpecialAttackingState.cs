using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpecialAttackingState : PlayerAttackingState
{
    private bool _hasInputAttack;
    private float _canAttackTimeRate;
    private float _canInputTimeRate;
    
    public PlayerSpecialAttackingState(PlayerCombatStateMachine stateMachine) : base(stateMachine)
    {
        _canAttackTimeRate = _combatData.SpecialAttackData.CanAttackTimeRate;
        _canInputTimeRate = _combatData.SpecialAttackData.CanInputTimeRate;
    }
    

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.ReusableData.ShouldRotate = false;
    }

    public override void Update()
    {
        base.Update();

        if (!CheckAnimation()) return;
        
        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < _canAttackTimeRate) // 还没到可以攻击的时候
            return;
        
        // 到了可以攻击的时候了
        if (_hasInputAttack)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerNormalAttackingState);
        }
    }

    public override void Exit()
    {
        _hasInputAttack = false;
        base.Exit();
    }

    public override void OnAnimationTransition()
    {
        _stateMachine.SetToIdleState();
    }

    #endregion
    

    #region Reusable Methods

    protected override void SetEnterAnimationParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.SpecialAttackID, true);
    }

    protected override void SetExitAnimationParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.SpecialAttackID, false);
    }

    #endregion


    #region Main Methods

    private bool CheckAnimation()
    {
        if (_stateMachine.Player.Animator.IsInTransition(0)) 
            return false;
        if (!_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).IsName("SpecialAttack"))
            return false;
        return true;
    }

    #endregion
    

    #region InputActions

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (!CheckAnimation()) return;
        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < _canInputTimeRate) // 还没到可以输入的时候
            return;
        _hasInputAttack = true;
    }

    protected override void OnSpecialAttackStarted(InputAction.CallbackContext context)
    {
        
    }

    #endregion
}