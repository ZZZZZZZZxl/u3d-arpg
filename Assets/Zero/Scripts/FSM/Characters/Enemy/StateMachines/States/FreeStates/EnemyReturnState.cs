using GGG.Tool;
using UnityEngine;

public class EnemyReturnState:EnemyState
{
    public EnemyReturnState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        
        // DevelopmentToos.WTF("敌人开始走回原来位置");
    }

    public override void Update()
    {
        base.Update();
        

        MoveToOriginPositon();
        Move();
        
        // DevelopmentToos.WTF((_stateMachine.ReusableData.TargetDirection));
        if ((_stateMachine.Enemy.transform.position - _stateMachine.ReusableData.OriginPosition).sqrMagnitude < 1f)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    #endregion


    #region Main Methods
    
    private void MoveToOriginPositon()
    {
        Vector3 moveDirection = DevelopmentToos.DirectionForTarget(_stateMachine.Enemy.transform, _stateMachine.ReusableData.OriginPosition);
        _stateMachine.ReusableData.TargetDirection.x = moveDirection.x;
        _stateMachine.ReusableData.TargetDirection.y = moveDirection.z;
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