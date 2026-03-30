using System.Collections.Generic;
using System.Linq;
using GGG.Tool;
using UnityEngine;

public class EnemyCombatState : EnemyState
{
    private const float MatchTargetNormalizedTime = 0.35f;
    private const float WallRaycastHeight = 0.5f;
    private const float TargetModifyTolerance = 1f;

    private Transform Weapon => Enemy.Weapon;

    private readonly List<AttackDistanceRow> _attackDistance;
    private readonly float _canAttackAfterTimeRate;
    private int _attackIndex;
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
        StopNavigation(true);
        base.Enter();

        _attackIndex = 0;
        _canAttack = false;
        _lastWeaponPos = Weapon.position;
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
        TryModifyPositionForWall();
        
        TryModifyPositionForTarget();

        if (!_canAttack || _stateMachine.ReusableData.TargetPlayer == null)
            return;

        var player = _stateMachine.ReusableData.TargetPlayer.GetComponent<Player>();
        if (player == null)
            return;

        Vector3 direction = ((_stateMachine.ReusableData.TargetPlayer.position + 0.5f * _stateMachine.ReusableData.TargetPlayer.up) - Weapon.position).normalized;

        if (!Physics.Raycast(Weapon.position, direction, out var hit, 1.5f))
            return;

        Transform target = hit.transform;
        while (target.parent != null)
            target = target.parent;

        if (!target.CompareTag("Player"))
            return;

        TriggerDamageToPlayer(player);
        _canAttack = false;
    }

    #endregion


    public void OnDrawGizmos()
    {
        if (Weapon == null || _stateMachine?.ReusableData?.TargetPlayer == null)
            return;

        Vector3 start = Weapon.position;
        Vector3 end = _stateMachine.ReusableData.TargetPlayer.position;
        Vector3 dir = (end - start).normalized;
        float distance = 1f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, start + dir * distance);

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
        Vector3 direction = DevelopmentToos.DirectionForTarget(_stateMachine.ReusableData.TargetPlayer.transform, Enemy.transform);

        player.SetHitInfo(Enemy.transform, damage, direction, heavy);
    }

    private bool CheckAnimator()
    {
        if (Enemy.Animator.IsInTransition(0))
            return false;

        return Enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _canAttackAfterTimeRate;
    }

    private void UpdateAttack()
    {
        if (!CheckAnimator())
            return;

        UpdateAttackInfo();

        if (_attackIndex == _attackDistance.Count)
            return;

        if (AttackDistanceDetection())
            _stateMachine.Enemy.Animator.SetTrigger(AnimationID.NormalAttackID);
    }

    private void UpdateAttackInfo()
    {
        UpdateAttackIndex();

        if (_attackIndex == _attackDistance.Count)
            return;

        UpdateAnimatorInNormalAttack();
        UpdateReusableAttackDistance();
    }

    private void UpdateReusableAttackDistance()
    {
        var distances = _attackDistance[_attackIndex];
        float usableDistance = distances.values.Max();
        Enemy.Animator.SetBool(AnimationID.NearAtttackID, false);

        foreach (float distance in distances.values)
        {
            if (!AttackDistanceDetection(distance))
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
        _attackIndex++;
        if (_attackIndex == _attackDistance.Count)
            _stateMachine.ChangeState(_stateMachine.IdleState);
    }

    private void TryModifyPositionForWall()
    {
        float attackDistance = Mathf.Max(_stateMachine.ReusableData.AttackDistance, 1f);
        Vector3 rayOrigin = Enemy.transform.position + Vector3.up * WallRaycastHeight;

        if (!Physics.Raycast(rayOrigin, Enemy.transform.forward, out var hitInfo, attackDistance))
            return;

        if (!hitInfo.collider.CompareTag("Envs"))
            return;

        Vector3 hitPoint = hitInfo.point;
        hitPoint.y -= WallRaycastHeight;

        ExecuteModify(hitPoint - Enemy.transform.forward * attackDistance, 0.5f);
    }

    private void TryModifyPositionForTarget()
    {
        if (!CheckDistanceNeedModify())
            return;

        if (Enemy.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > MatchTargetNormalizedTime)
            return;

        Transform targetPlayer = _stateMachine.ReusableData.TargetPlayer;
        float attackDistance = _stateMachine.ReusableData.AttackDistance;

        ExecuteModify(targetPlayer.position - Enemy.transform.forward * (attackDistance * 0.8f), 0.05f);
    }

    private bool CheckDistanceNeedModify()
    {
        Transform targetPlayer = _stateMachine.ReusableData.TargetPlayer;
        if (targetPlayer == null)
            return false;
        if (_stateMachine.ReusableData.AttackDistance > 20f)
            return false;

        float attackDistance = _stateMachine.ReusableData.AttackDistance;
        float distance = DevelopmentToos.DistanceForTarget(targetPlayer, Enemy.transform);

        if (distance > attackDistance && distance < attackDistance + TargetModifyTolerance)
            return true;
        
        if (distance < attackDistance)
            return true;

        return false;
    }

    private void ExecuteModify(Vector3 targetPosition, float normalizeTime = MatchTargetNormalizedTime)
    {
        if (Enemy.Animator.isMatchingTarget || Enemy.Animator.IsInTransition(0))
            return;

        Enemy.Animator.MatchTarget(
            targetPosition,
            Quaternion.identity,
            AvatarTarget.Root,
            new MatchTargetWeightMask(Vector3.one, 0f),
            0f,
            normalizeTime
        );
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
