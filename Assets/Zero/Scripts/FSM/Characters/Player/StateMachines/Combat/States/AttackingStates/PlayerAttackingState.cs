using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackingState : PlayerCombatState
{
    protected Transform WeaponBase => _stateMachine.Player.WeaponBase;
    protected Transform WeaponTop => _stateMachine.Player.WeaponTop;
    
    public PlayerAttackingState(PlayerCombatStateMachine stateMachine) : base(stateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        _stateMachine.ReusableData.CanAttack = false;
    }

    public override void Update()
    {
        base.Update();
        Attack();
        LookAtEnemy();
    }

    public override void Exit()
    {
        _stateMachine.ReusableData.CanAttack = false;
        base.Exit();
    }

    // public override void PhysicsUpdate()
    // {
    //     base.PhysicsUpdate();
    //
    //     Vector3 start = WeaponBase.position + (WeaponTop.position - WeaponBase.position) / 3;
    //     Vector3 end = WeaponTop.position;
    //     float radius = .1f;
    //
    //     
    //     // 检测碰撞
    //     Collider[] results = new Collider[1];
    //     var size = Physics.OverlapCapsuleNonAlloc(start, end, radius, results);
    //     if (size == 0)
    //         return;
    //
    //     var hit = results[0];
    //     Transform obj = hit.transform;
    //     while (obj.parent != null)
    //     {
    //         obj = obj.parent;
    //     }
    //
    //     
    //     if (obj.CompareTag("Envs") )
    //     {
    //         DevelopmentToos.WTF("碰到建筑物了");
    //         if (Physics.Raycast(_stateMachine.Player.transform.position, _stateMachine.Player.transform.forward,
    //                 out var hitInfo, 1f))
    //         {
    //             Vector3 hitPoint = hitInfo.point;
    //             
    //             ExcuteModify(hitPoint - _stateMachine.Player.transform.forward * _combatData.AttackDistance, .5f);
    //             DevelopmentToos.WTF("调整位置了");
    //         }
    //     }
    //     if (obj.CompareTag("Enemy") && _stateMachine.ReusableData.CurrentEnemy != null)
    //     {
    //         StartDamage();
    //     }
    // }
    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Physics.Raycast(_stateMachine.Player.transform.position + 0.5f * Vector3.up, _stateMachine.Player.transform.forward,
                out var hitInfo, 1f))
        {
            if (hitInfo.collider.CompareTag("Envs"))
            {
                Vector3 hitPoint = hitInfo.point;
                hitPoint.y -= 0.5f;
                ExcuteModify(hitPoint - _stateMachine.Player.transform.forward * _combatData.AttackDistance, .5f);
            }
        }
    }


    #endregion


    #region Reusable Methods

    protected virtual void Attack()
    {
        if (!_stateMachine.ReusableData.CanAttack)
            return;

        if (!_stateMachine.ReusableData.CurrentEnemy)
            return;

        if (DevelopmentToos.DistanceForTarget(_stateMachine.ReusableData.CurrentEnemy, _stateMachine.Player.transform) >
            _combatData.AttackDistance)
            return;

        if (!DevelopmentToos.IsTargetAtFront(_stateMachine.ReusableData.CurrentEnemy, _stateMachine.Player.transform,
                _combatData.FrontAreaAngle))
            return;

        StartDamage();
    }

    protected virtual void StartDamage()
    {   
        if (_stateMachine.ReusableData.CanAttack)
        {
            _stateMachine.ReusableData.CanAttack = false;
            TriggerAttackDamage();
        }
    }
    
    protected void ExcuteModify(Vector3 targetPosition, float normalizeTime = 0.35f)
    {
        
        if (!_stateMachine.Player.Animator.isMatchingTarget && !_stateMachine.Player.Animator.IsInTransition(0))
        {
            _stateMachine.Player.Animator.MatchTarget(
                targetPosition,
                Quaternion.identity, AvatarTarget.Root,
                new MatchTargetWeightMask(Vector3.one, 0f),
                0f, normalizeTime);
        }
    }
    
    protected virtual void LookAtEnemy()
    {
        if (!_stateMachine.ReusableData.CurrentEnemy) return;
        
        _stateMachine.Player.transform.Look(_stateMachine.ReusableData.CurrentEnemy.position, 5000f);
        
        Vector3 direction = _stateMachine.ReusableData.CurrentEnemy.position - _stateMachine.Player.transform.position;
        direction.y = 0f;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
        _stateMachine.ReusableData.CurrentTargetRotation = targetAngle;
        _stateMachine.ReusableData.DampedTargetRotationPassedTime = 0f;
    }
    
    protected virtual void TriggerAttackDamage( )
    {
        ApplyHitToCurrentEnemy();
    }
    
    
    protected virtual void ApplyHitToCurrentEnemy()
    {
        Transform enemyTransform = _stateMachine.ReusableData.CurrentEnemy;

        Vector3 hitDirection = DevelopmentToos.DirectionForTarget(enemyTransform, _stateMachine.Player.transform);

        Enemy enemy = enemyTransform.GetComponentInParent<Enemy>();
        
        enemy.EnemyStateMachine.SetHitInfo(_stateMachine.Player.transform, _stateMachine.ReusableData.Damage,
            hitDirection, _stateMachine.ReusableData.Heavy);

        GameEventManager.MainInstance.Call(
            EventNames.TakeDamager,
            _stateMachine.ReusableData.Damage,
            enemyTransform
        );
    }

    #endregion

}