using System.Collections.Generic;
using System.Linq;
using GGG.Tool;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCombatState:EnemyState
{
    private Transform Weapon => Enemy.Weapon;
    
    private List<AttackDistanceRow> _attackDistance;
    private int _attackIndex;
    private float _canAttackAfterTimeRate;
    private bool _canAttack;
    private Vector3 _lastWeaponPos;
    
    public EnemyCombatState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _attackDistance = _aiActionData.EnemyCombatData.AttackDistance;
        _canAttackAfterTimeRate = _aiActionData.EnemyCombatData.CanAttackAfterTimeRate;
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _attackIndex = 0;
        _canAttack = false;
        _lastWeaponPos = Weapon.position;
        // DevelopmentToos.WTF("敌人开始攻击了");
    }

    public override void Update()
    {
        base.Update();

        UpdateAttack();
    }

    public override void Exit()
    {
        base.Exit();

        _stateMachine.ReusableData.AttackDistance = _aiActionData.AttackDistance;
    }

    public override void OnAnimationTransition()
    {
        _stateMachine.ChangeState(_stateMachine.CombatEndState);
    }

    public override void OnAnimationEvent()
    {
        ResetCanAttack();
    }

    public override void PhysicsUpdate()
    {
        if (!_canAttack)
            return;
        
        if (_stateMachine.ReusableData.TargetPlayer == null)
            return;
        
        var player = _stateMachine.ReusableData.TargetPlayer.GetComponent<Player>();
        if (player == null)
            return;

        if (Physics.Raycast(Weapon.position,
                ((_stateMachine.ReusableData.TargetPlayer.position + 0.5f*_stateMachine.ReusableData.TargetPlayer.up) - Weapon.transform.position).normalized, out var hit,
                1.5f))
        {
            Transform t = hit.transform;
            while (t.parent != null)
                t = t.parent;

            if (t.CompareTag("Player"))
            {
                TriggerDamageToPlayer(player);
                _canAttack = false;
            }
        }
    }

    #endregion


    public void OnDrawGizmos()
    {
        if (Weapon == null || _stateMachine?.ReusableData?.TargetPlayer == null)
            return;

        Vector3 start = Weapon.position;
        Vector3 end = _stateMachine.ReusableData.TargetPlayer.position ;
        Vector3 dir = (end - start).normalized;

        // 射线长度（你 Physics.Raycast 里用的距离）
        float distance = 1f;

        // 画射线
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, start + dir * distance);

        // 可选：画射线终点球
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(start + dir * distance, 0.1f);
    }
    
    #region Main Methods

    private void ResetCanAttack()
    {
        _canAttack = true;
    }
    
    private void TriggerDamageToPlayer(Player player)
    {
        float damage = _aiActionData.EnemyCombatData.Damage[_attackIndex];
        bool heavy = _aiActionData.EnemyCombatData.HeavyAttack[_attackIndex];
        Vector3 derection =
            DevelopmentToos.DirectionForTarget(_stateMachine.ReusableData.TargetPlayer.transform, Enemy.transform);
        
        player.SetHitInfo(Enemy.transform, damage, derection, heavy);  
    }
    
    private bool CheckAnimator()
    {
        if (Enemy.Animator.IsInTransition(0))
            return false;
        if (Enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < _canAttackAfterTimeRate)
            return false;
        
        return true;
    }
    
    private void UpdateAttack()
    {
        if (!CheckAnimator())
            return;
        
        // 先获得下一段攻击的信息
        UpdateAttackInfo();
        
        if (_attackIndex == _attackDistance.Count)
            return;
        
        // 执行攻击
        if (AttackDistanceDetection())
        {
            _stateMachine.Enemy.Animator.SetTrigger(AnimationID.NormalAttackID);
        }
    }

    private void UpdateAttackInfo()
    {
        // 更新下一个攻击的index
        UpdateAttackIndex();

        if (_attackIndex == _attackDistance.Count)
            return;
        // 更新攻击状态
        UpdateAnimatorInNormalAttack();

        // 更新下一代段攻击的攻击距离
        UpdateReusableAttackDistance();
    }

    private void UpdateReusableAttackDistance()
    {
        // 默认挑选最远的一个攻击，如果有更近的能打到敌人的就用更近的，然后切换nearAttack
        var distances = _attackDistance[_attackIndex];
        float usableDistance = distances.values.Max();
        Enemy.Animator.SetBool(AnimationID.NearAtttackID, false);

        foreach (var distancesValue in distances.values)
        {
            if (! AttackDistanceDetection(distancesValue))
                return;
            
            Enemy.Animator.SetBool(AnimationID.NearAtttackID, true);
        }

        _stateMachine.ReusableData.AttackDistance = usableDistance;
    }

    private void UpdateAnimatorInNormalAttack()
    {
        if (_attackIndex != 0)
            _stateMachine.Enemy.Animator.SetBool(AnimationID.InNormalAttackID, true);
    }

    private void UpdateAttackIndex()
    {
        _attackIndex ++;
        if (_attackIndex == _attackDistance.Count)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }
    }

    
    


    #endregion


    #region Reusable Methods
    
    
    protected override void ReciveHit()
    {
        
    }

    protected override void SetAnimationEnterParameters()
    {
        _stateMachine.Enemy.Animator.SetTrigger(AnimationID.NormalAttackID);
    }

    protected override void SetAnimationExitParameters()
    {
        _stateMachine.Enemy.Animator.SetBool(AnimationID.NormalAttackID, false);
        _stateMachine.Enemy.Animator.SetBool(AnimationID.InNormalAttackID, false);
        _stateMachine.Enemy.Animator.SetBool(AnimationID.NearAtttackID, false);
    }

    protected override void ChangeCombatState()
    {
        
    }

    protected override void ChangeIdleState()
    {
        
    }

    #endregion
    
    
}