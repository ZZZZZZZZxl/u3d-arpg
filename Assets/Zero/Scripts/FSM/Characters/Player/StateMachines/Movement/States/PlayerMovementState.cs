using GGG.Tool;
using UnityEngine;

public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine _stateMachine;
    protected PlayerGroundedData _movementData;

    public PlayerMovementState(PlayerMovementStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _movementData = _stateMachine.Player.Data.GroundedData;
    }
    
    #region IState Methods

    public virtual void Enter()
    {
        // DevelopmentToos.WTF(GetType().Name);
        SetAnimatorParameters();
    }

    public virtual void Exit()
    {
        
    }

    public virtual void Update()
    {
        ReceiveHit();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
        SetAnimatorMovement();
    }

    public virtual void PhysicsUpdate()
    {
        Move();
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

    private void SetAnimatorMovement()
    {
    }

    private void ReadMovementInput()
    {
        _stateMachine.ReusableData.MovementInput =
            PlayerInputManager.MainInstance.PlayerActions.Movement.ReadValue<Vector2>();
    }
    
    private void Move()
    {
        Vector3 moveDirection = GetMoveDirection();
        moveDirection = DevelopmentToos.ModifyDirectionOnSlope(moveDirection, _stateMachine.Player.transform,
            _stateMachine.Player.Controller.height * 0.85f, _movementData.GroundLayer);
        _stateMachine.Player.Controller.Move(moveDirection * Time.deltaTime);
    }
    
    private void ReceiveHit()
    {
        if (!_stateMachine.ReusableData.ReciveHit) return;
        
        _stateMachine.ReusableData.ReciveHit = false;
        _stateMachine.SetToHitState();
    }

    #endregion


    #region Reusable Methods

    protected Vector3 GetMoveDirection()
    {
        return _stateMachine.Player.MoveDirection;
    }
    
    protected virtual void SetAnimatorParameters()
    {
        
    }
    

    #endregion
}