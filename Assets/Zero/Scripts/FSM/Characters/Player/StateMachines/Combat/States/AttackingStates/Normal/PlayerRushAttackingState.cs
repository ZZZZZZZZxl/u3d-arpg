using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRushAttackingState : PlayerAttackingState
{
    private float _canAttackTimeRate;
    
    public PlayerRushAttackingState(PlayerCombatStateMachine stateMachine) : base(stateMachine)
    {
        _canAttackTimeRate = _combatData.RushAttackingData.CanTransiteTimeRate;
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        InitData();
    }

    public override void Update()
    {
        base.Update();
        
        Attack();
    }

    private void InitData()
    {

        _stateMachine.ReusableData.CanDamage = true;
        
        _stateMachine.ReusableData.Damage = _combatData.RushAttackingData.Damage;
        _stateMachine.ReusableData.Heavy = _combatData.RushAttackingData.Heavy;
        
        _stateMachine.ReusableData.ShouldRotate = false;
        _stateMachine.ReusableData.MovementSpeedModifier = 
            _stateMachine.Player.Data.GroundedData.PlayerSprintData.SpeedModifier;
    }

    public override void OnAnimationTransition()
    {
        _stateMachine.ChangeState(_stateMachine.PlayerRushAttackingEndState);
    }

    #endregion


    #region Main Methods
    
    private bool CheckTransiteTime()
    {
        if (!CheckAnimation()) return false;
        
        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _canAttackTimeRate)
            return true;
        return false;
    }
    
    private bool CheckAnimation()
    {
        if (_stateMachine.Player.Animator.IsInTransition(0)) 
            return false;
        if (!_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).IsName("rush"))
            return false;
        return true;
    }

    #endregion
    

    #region Resumable Methods

    protected override void SetEnterAnimationParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.AttackID, true);
        _stateMachine.Player.Animator.SetBool(AnimationID.InRushingAttackID, true);
    }

    protected override void SetExitAnimationParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.AttackID, false);
        _stateMachine.Player.Animator.SetBool(AnimationID.InRushingAttackID, false);
    }

    #endregion


    #region InputActions

    protected override void OnSpecialAttackStarted(InputAction.CallbackContext context)
    {
        if (!CheckTransiteTime()) return;
        base.OnSpecialAttackStarted(context);
    }

    protected override void OnFinalAttackStarted(InputAction.CallbackContext context)
    {
        if (!CheckTransiteTime()) return;
        base.OnFinalAttackStarted(context);
    }

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (!CheckTransiteTime()) return;
        base.OnAttackStarted(context);
    }

    #endregion
}