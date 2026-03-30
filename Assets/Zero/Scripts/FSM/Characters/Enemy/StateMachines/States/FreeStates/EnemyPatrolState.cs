using GGG.Tool;
using UnityEngine;

public class EnemyPatrolState:EnemyState
{
    private readonly EnemyPatrolData _data;
    private Vector2Int[] _moveDirections;
    private int _moveIndex;
    private float _moveDistance;
    
    public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _data = _aiActionData.EnemyPatrolData;
        _moveDirections = _data.MoveDirections;
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        
        // DevelopmentToos.WTF("敌人开始四处巡逻");

        _moveDistance = 0f;
        SetTargetDirection();
    }

    public override void Update()
    {
        base.Update();

        MoveWithDirection();
    }

    #endregion


    #region Main Methods

    private void SetTargetDirection()
    {
        _stateMachine.ReusableData.TargetDirection = _moveDirections[_moveIndex];
        _moveIndex ++;
        if (_moveIndex == _moveDirections.Length)
            _moveIndex = 0;
    }
    
    private void MoveWithDirection()
    {
        _moveDistance += (_stateMachine.ReusableData.MoveDirection).magnitude;
        Move();

        if (_moveDistance >= _data.PatrolDistance)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    #endregion


    #region Reusable Methods

    // 移动的parameters
    protected override void SetAnimationEnterParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.WalkID, true);
    }

    protected override void SetAnimationExitParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.WalkID, false);
    }

    #endregion
}