using GGG.Tool;
using UnityEngine;

public abstract class EnemyState : IState
{
    protected readonly EnemyStateMachine _stateMachine;
    protected readonly AIActionData _aiActionData;

    protected EnemyState(EnemyStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _aiActionData = stateMachine.Enemy.Data;
    }

    protected Enemy Enemy => _stateMachine.Enemy;

    #region IState Methods

    public virtual void Enter()
    {
        // DevelopmentToos.WTF("IState Enter: " + GetType().Name);
        SetAnimationEnterParameters();
    }

    public virtual void Exit()
    {
        SetAnimationExitParameters();
    }

    public virtual void Update()
    {
        PlayerDetection();
        ActionWithTargetPlayer();
        ReciveHit();
    }

    public virtual void HandleInput()
    {
        ReadMoveDirection();
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

    protected virtual void ReciveHit()
    {
        if (!_stateMachine.ReusableData.ReciveHit)
            return;

        _stateMachine.ReusableData.ReciveHit = false;
        ChangeHitState();
    }

    private void ReadMoveDirection()
    {
        // _stateMachine.ReusableData.MoveDirection = DevelopmentToos.ModifyDirectionOnSlope(
        //     _stateMachine.Enemy.MoveDirection,
        //     _stateMachine.Enemy.transform,
        //     _stateMachine.Enemy.Controller.height * 0.85f,
        //     _aiActionData.GroundLayer
        // );
        _stateMachine.ReusableData.MoveDirection = _stateMachine.Enemy.MoveDirection;
    }

    private void HandleCombatAndApproach()
    {
        if (!AttackDistanceDetection())
        {
            ChangeApproachState();
            return;
        }

        if (_stateMachine.ReusableData.LastAttackTime == 0f)
        {
            _stateMachine.ReusableData.LastAttackTime = Time.time;
            ChangeCombatState();
            return;
        }

        if (_stateMachine.ReusableData.LastAttackTime + _aiActionData.AttackColdTime < Time.time)
        {
            _stateMachine.ReusableData.LastAttackTime = Time.time;
            ChangeCombatState();
            return;
        }

        ChangeIdleState();
    }

    private void TryToReturnState()
    {
        if (DevelopmentToos.DistanceForTarget(_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform) > _aiActionData.DetectionDistance)
        {
            _stateMachine.ReusableData.TargetPlayer = null;
            ChangeReturnState();
        }
    }

    #endregion


    #region Reusable Methods

    protected virtual void ChangeDieState()
    {
        _stateMachine.ChangeState(_stateMachine.DeadState);
    }

    protected virtual void ChangeHitState()
    {
        _stateMachine.ChangeState(_stateMachine.HitState);
    }

    protected virtual void ChangePatrolState()
    {
        _stateMachine.ChangeState(_stateMachine.PatrolState);
    }

    protected virtual void ChangeApproachState()
    {
        _stateMachine.ChangeState(_stateMachine.ApproachState);
    }

    protected virtual void ChangeCombatState()
    {
        _stateMachine.ChangeState(_stateMachine.CombatState);
    }

    protected virtual void ChangeReturnState()
    {
        _stateMachine.ChangeState(_stateMachine.ReturnState);
    }

    protected virtual void ChangeIdleState()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    protected virtual void SetAnimationEnterParameters()
    {
    }

    protected virtual void SetAnimationExitParameters()
    {
    }

    protected virtual void ActionWithTargetPlayer()
    {
        if (!_stateMachine.ReusableData.TargetPlayer)
            return;

        HandleCombatAndApproach();
        TryToReturnState();
    }

    protected virtual void PlayerDetection()
    {
        if (_stateMachine.ReusableData.TargetPlayer)
            return;

        Transform nearestPlayer = GetNearestPlayer();
        if (nearestPlayer == null)
            return;

        if (DevelopmentToos.DistanceForTarget(nearestPlayer, _stateMachine.Enemy.transform) > _aiActionData.DetectionDistance)
            return;

        if (!DevelopmentToos.IsTargetAtFront(nearestPlayer, _stateMachine.Enemy.transform, 180f))
            return;

        Transform root = nearestPlayer.transform;
        while (root.parent != null)
            root = root.parent;

        _stateMachine.ReusableData.TargetPlayer = root;
    }

    protected virtual bool AttackDistanceDetection(float attackDistance = -1f)
    {
        if (_stateMachine.ReusableData.TargetPlayer == null)
            return false;

        if (attackDistance < 0f)
        {
            if (DevelopmentToos.DistanceForTarget(_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform) > _stateMachine.ReusableData.AttackDistance + .2f)
                return false;

            if (!DevelopmentToos.IsTargetAtFront(_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform, 240f))
                return false;

            Vector3 direction = DevelopmentToos.DirectionForTarget(Enemy.transform, _stateMachine.ReusableData.TargetPlayer.transform);
            _stateMachine.ReusableData.TargetDirection = new Vector2(direction.x, direction.z);
            return true;
        }

        return DevelopmentToos.DistanceForTarget(_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform) <= attackDistance + .2f
               && DevelopmentToos.IsTargetAtFront(_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform, 180f);
    }

    protected Transform GetNearestPlayer()
    {
        GameObject[] players = ObjectsManager.MainInstance.Players;

        float minDistance = -1f;
        Transform nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = DevelopmentToos.DistanceForTarget(_stateMachine.Enemy.transform, player.transform);

            if (nearestPlayer == null || distance < minDistance)
            {
                nearestPlayer = player.transform;
                minDistance = distance;
            }
        }

        return nearestPlayer;
    }

    protected bool UpdateNavigationDirection()
    {
        Vector2 steeringDirection = _stateMachine.Enemy.GetSteeringDirection();

        if (steeringDirection == Vector2.zero)
            return false;

        _stateMachine.ReusableData.TargetDirection = steeringDirection;
        return true;
    }

    protected void StopNavigation(bool clearDirection = false)
    {
        _stateMachine.Enemy.StopAgent();

        if (clearDirection)
            _stateMachine.ReusableData.TargetDirection = Vector2.zero;
    }

    protected void Move()
    {
        _stateMachine.Enemy.Controller.Move(_stateMachine.ReusableData.MoveDirection * Time.deltaTime);
        _stateMachine.Enemy.SyncAgentPosition();
    }

    #endregion
}
