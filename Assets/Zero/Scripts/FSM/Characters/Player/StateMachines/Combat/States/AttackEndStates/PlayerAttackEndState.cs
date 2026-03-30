using UnityEngine;

public class PlayerAttackEndState : PlayerCombatState
{
    public PlayerAttackEndState(PlayerCombatStateMachine stateMachine) : base(stateMachine)
    {
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
        
        if (_stateMachine.ReusableData.MovementInput != Vector2.zero)
            StartMoving();
    }

    public override void OnAnimationTransition() // 结束动画播放到最后切idle
    {
        _stateMachine.SetToIdleState();
    }

    #endregion


    #region Reusable Methods

    protected virtual void StartMoving()
    {
        _stateMachine.SetToStartRunningState();
    }

    #endregion
}