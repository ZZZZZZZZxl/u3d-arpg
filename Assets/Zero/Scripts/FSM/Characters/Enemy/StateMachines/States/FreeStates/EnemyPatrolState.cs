using GGG.Tool;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyState
{
    private readonly EnemyPatrolData _data;
    private Vector3 _currentPatrolPoint;

    public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _data = _aiActionData.EnemyPatrolData;
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        TrySetPatrolDestination();
    }

    public override void Update()
    {
        base.Update();

        if (_stateMachine.ReusableData.TargetPlayer != null)
            return;

        if (!UpdateNavigationDirection())
        {
            if (_stateMachine.Enemy.HasReachedDestination())
                _stateMachine.ChangeState(_stateMachine.IdleState);

            return;
        }

        Move();

        if (_stateMachine.Enemy.HasReachedDestination())
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    #endregion


    #region Main Methods

    private bool TrySetPatrolDestination()
    {
        Vector3 center = _stateMachine.ReusableData.OriginPosition;
        float sampleDistance = Mathf.Max(1f, _data.PatrolDistance * 0.5f);

        for (int i = 0; i < 6; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * _data.PatrolDistance;
            Vector3 candidate = center + new Vector3(randomOffset.x, 0f, randomOffset.y);

            if (!NavMesh.SamplePosition(candidate, out var hit, sampleDistance, NavMesh.AllAreas))
                continue;

            _currentPatrolPoint = hit.position;
            return _stateMachine.Enemy.SetDestination(_currentPatrolPoint);
        }

        _currentPatrolPoint = center;
        return _stateMachine.Enemy.SetDestination(_currentPatrolPoint);
    }

    #endregion


    #region Reusable Methods

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
