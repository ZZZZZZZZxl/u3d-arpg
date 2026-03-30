using System.Net.Sockets;
using GGG.Tool;
using UnityEditor;
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
        // DevelopmentToos.WTF("Enter " + GetType().Name);
        SetAnimationEnterParameters();
    }

    public virtual void Exit()
    {
        SetAnimationExitParameters();
    }

    public virtual void Update()
    {
        RotateToPlayer();
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
        if (!_stateMachine.ReusableData.ReciveHit) return;
        
        _stateMachine.ReusableData.ReciveHit = false;
        ExecuteChangeState();
    }

    private void ExecuteChangeState()
    {
        ChangeHitState();
    }
    
    
    private void ReadMoveDirection()
    {
        _stateMachine.ReusableData.MoveDirection = DevelopmentToos.ModifyDirectionOnSlope(
            _stateMachine.Enemy.MoveDirection, _stateMachine.Enemy.transform,
            _stateMachine.Enemy.Controller.height * 0.85f, _aiActionData.GroundLayer);
    }
    
    private void HandleCombatAndApproach()
    {
        // 检测到敌人了，它在攻击范围内与否的转换逻辑
        if (!AttackDistanceDetection())
        {
            ChangeApproachState();
        }
        else
        {
            // 1. 之前没攻击过可以攻击
            if (_stateMachine.ReusableData.LastAttackTime == 0f)
            {
                _stateMachine.ReusableData.LastAttackTime = Time.time;
                ChangeCombatState();
                return;
            }
                
            // 2. 之前攻击过 但是冷却时间到了 可以攻击
            if (_stateMachine.ReusableData.LastAttackTime + _aiActionData.AttackColdTime < Time.time)
            {
                _stateMachine.ReusableData.LastAttackTime = Time.time;
                ChangeCombatState();
            }
            else
            {
                // 冷却时间没到 但是到攻击范围内了 就用Idle待命
                ChangeIdleState();
            }
        }
    }


    private void TryToReturnState()
    {
        // 目标敌人走出去了，切换回 走回原点状态
        if (DevelopmentToos.DistanceForTarget
                (_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform) 
            > _aiActionData.DetectionDistance)
        {
            _stateMachine.ReusableData.TargetPlayer = null;
            ChangeReturnState();
        }
    }

    private void RotateToPlayer()
    {
        if (_stateMachine.ReusableData.TargetPlayer == null)
            return;
        
        Enemy.transform.Look(_stateMachine.ReusableData.TargetPlayer.transform.position, 5000f);
        Vector3 moveDirection = DevelopmentToos.DirectionForTarget(Enemy.transform, _stateMachine.ReusableData.TargetPlayer.transform);
        moveDirection = DevelopmentToos.ModifyDirectionOnSlope(moveDirection, Enemy.transform, Enemy.Controller.height * 0.85f, _aiActionData.GroundLayer);
        _stateMachine.ReusableData.MoveDirection.x = moveDirection.x;
        _stateMachine.ReusableData.MoveDirection.y = moveDirection.z;
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
    
    // 当出现了目标敌人的时候，调用这个函数管理
    protected virtual void ActionWithTargetPlayer()
    {
        if (!_stateMachine.ReusableData.TargetPlayer)
            return;
        
        HandleCombatAndApproach();
        TryToReturnState();
    }

    protected virtual void ActionInFreeState()
    {
        _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    // 检测前方是否有玩家
    protected virtual void PlayerDetection()
    {
        // 范围内有目标了就不更新了
        if (_stateMachine.ReusableData.TargetPlayer)
            return;
        
        // 距离最近的一个player
        Transform nearestPlayer = GetNearestPlayer();
        if (nearestPlayer == null)
            return;
        
        // 检测前方距离内是否有玩家
        if (DevelopmentToos.DistanceForTarget(nearestPlayer, _stateMachine.Enemy.transform) <=
            _aiActionData.DetectionDistance &&
            DevelopmentToos.IsTargetAtFront(nearestPlayer, _stateMachine.Enemy.transform, 180f))
        {
            Transform t = nearestPlayer.transform;

            while (t.parent != null)
            {
                t = t.parent;
            }
            
            _stateMachine.ReusableData.TargetPlayer = t;
        }
    }

    
    protected virtual bool AttackDistanceDetection(float attackDistance = -1f)
    {
        if (_stateMachine.ReusableData.TargetPlayer == null)
        {
            return false;
        }

        if (attackDistance < 0)
        {
            if (DevelopmentToos.DistanceForTarget(_stateMachine.ReusableData.TargetPlayer,
                    _stateMachine.Enemy.transform) <=
                _stateMachine.ReusableData.AttackDistance &&
                DevelopmentToos.IsTargetAtFront(_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform,
                    240f))
            {
                _stateMachine.ReusableData.TargetDirection =
                    DevelopmentToos.DirectionForTarget(Enemy.transform, _stateMachine.ReusableData.TargetPlayer.transform);
            
                return true;
            }
        }
        else
        {
            if (DevelopmentToos.DistanceForTarget(_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform) <=
                attackDistance &&
                DevelopmentToos.IsTargetAtFront(_stateMachine.ReusableData.TargetPlayer, _stateMachine.Enemy.transform,
                    180f))
                return true;
        }

        return false;
    }

    protected Transform GetNearestPlayer()
    {
        GameObject[] player = ObjectsManager.MainInstance.Players;
        
        float minDistance = -1;
        Transform nearestPlayer = null;
        
        foreach (var item in player)
        {
            float distance = DevelopmentToos.DistanceForTarget(
                _stateMachine.Enemy.transform, item.transform);

            if (nearestPlayer == null)
            {
                nearestPlayer = item.transform;
                minDistance = distance;
            }

            if (distance < minDistance)
            {
                nearestPlayer = item.transform;
                minDistance = distance;
            }
        }
        
        return nearestPlayer;
    }

    protected void Move()
    {
        _stateMachine.Enemy.Controller.Move(_stateMachine.ReusableData.MoveDirection * Time.deltaTime);
    }

    
    #endregion
}
