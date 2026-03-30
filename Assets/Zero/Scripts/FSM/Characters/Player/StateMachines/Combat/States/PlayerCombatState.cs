using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatState : IState
{
    protected readonly PlayerCombatStateMachine _stateMachine;
    protected readonly CombatData _combatData;

    public PlayerCombatState(PlayerCombatStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _combatData = _stateMachine.Player.Data.CombatData;
    }
    
    
    #region IState Methods

    public virtual void Enter()
    {
        // DevelopmentToos.WTF(GetType().Name);

        AddInputActionsCallbacks();
        SetEnterAnimationParameters();
        _stateMachine.ReusableData.MovementSpeedModifier = 0f;
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
        SetExitAnimationParameters();
    }

    public virtual void Update()
    {
        CheckEnemyDie();
        RecevieHit();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void OnAnimationEnter()
    {
        
    }

    public virtual void OnAnimationExit()
    {
        
    }

    public virtual void OnAnimationTransition()
    {
        
    }

    public virtual void OnAnimationEvent()
    {
        
    }

    #endregion


    #region Main Methods

    private void RecevieHit()
    {
        if (!_stateMachine.ReusableData.ReciveHit) return;
        
        _stateMachine.ReusableData.ReciveHit = false;
        _stateMachine.ChangeState(_stateMachine.PlayerBeHittingState);
    }
    
    protected void ReadMovementInput()
    {
        _stateMachine.ReusableData.MovementInput =
            PlayerInputManager.MainInstance.PlayerActions.Movement.ReadValue<Vector2>();
    }
    
    private void CheckEnemyDie()
    {
        if (_stateMachine.ReusableData.CurrentEnemy)
        {
            var healthController = _stateMachine.ReusableData.CurrentEnemy.GetComponentInParent<EnemyHealthController>();

            if (healthController.IsDead)
            {
                _stateMachine.ReusableData.CurrentEnemy = null;
            }
        }
    }
    

    #endregion
    
    
    #region Resuable Methods

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInputManager.MainInstance.PlayerActions.Attack.started += OnAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Special.started += OnSpecialAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Final.started += OnFinalAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Dash.started += OnDashStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInputManager.MainInstance.PlayerActions.Attack.started -= OnAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Special.started -= OnSpecialAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Final.started -= OnFinalAttackStarted;
        PlayerInputManager.MainInstance.PlayerActions.Dash.started -= OnDashStarted;
    }
    
    protected virtual void SetEnterAnimationParameters()
    {
        
    }

    protected virtual void SetExitAnimationParameters()
    {
        
    }
    


    #endregion


    #region InputActions

    protected virtual void OnFinalAttackStarted(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerFinalAttackingState);
    }

    protected virtual void OnSpecialAttackStarted(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerSpecialAttackingState);
    }

    protected virtual void OnAttackStarted(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerNormalAttackingState);
    }
    
    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        _stateMachine.Player.Animator.SetTrigger(AnimationID.DashID);
        _stateMachine.SetToDashingState();
    }    

    #endregion
}