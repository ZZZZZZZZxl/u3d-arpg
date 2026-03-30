using GGG.Tool;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private EnemyIdleData _data;
    private float _startTime;
    
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _data = _aiActionData.EnemyIdleData;
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        
        // DevelopmentToos.WTF("敌人进入静止状态");
        
        _startTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (_startTime + _data.TimeToPatrol > Time.time)
        {
            return;
        }

        if (DevelopmentToos.DistanceForTarget(_stateMachine.Enemy.transform,
                _stateMachine.ReusableData.OriginPosition) > 2f)
        {
            _stateMachine.ChangeState(_stateMachine.ReturnState);
        }
        else
        {
            _stateMachine.ChangeState(_stateMachine.PatrolState);
        }
    }

    #endregion


    #region Reusable Methods

    protected override void ChangeIdleState()
    {
        
    }

    #endregion
    
}
