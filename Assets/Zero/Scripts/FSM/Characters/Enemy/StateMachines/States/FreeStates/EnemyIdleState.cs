using GGG.Tool;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private readonly EnemyIdleData _data;
    private float _startTime;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _data = _aiActionData.EnemyIdleData;
    }

    #region IState Methods

    public override void Enter()
    {
        StopNavigation(true);
        base.Enter();
        _startTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (_startTime + _data.TimeToPatrol > Time.time)
            return;

        if (DevelopmentToos.DistanceForTarget(_stateMachine.Enemy.transform, _stateMachine.ReusableData.OriginPosition) > 2f)
        {
            _stateMachine.ChangeState(_stateMachine.ReturnState);
            return;
        }

        _stateMachine.ChangeState(_stateMachine.PatrolState);
    }

    #endregion


    #region Reusable Methods

    protected override void ChangeIdleState()
    {
    }

    #endregion
}
