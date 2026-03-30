using GGG.Tool;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    private readonly Enemy _enemy;
    private readonly EnemyIdleState _idleState;
    private readonly EnemyHitState _hitState;
    private readonly EnemyDeadState _deadState;
    private readonly EnemyPatrolState _patrolState;
    private readonly EnemyApproachState _approachState;
    private readonly EnemyCombatState _combatState;
    private readonly EnemyReturnState _returnState;
    private readonly EnemyCombatEndState _combatEndState;
    
    private EnemyReusableData _reusableData;

    public EnemyStateMachine(Enemy enemy)
    {
        _enemy = enemy;
        _reusableData = enemy.ReusableData;
        _reusableData.AttackDistance = enemy.Data.AttackDistance;
        _idleState = new EnemyIdleState(this);
        _hitState = new EnemyHitState(this);
        _deadState = new EnemyDeadState(this);
        _patrolState = new EnemyPatrolState(this);
        _approachState = new EnemyApproachState(this);
        _combatState = new EnemyCombatState(this);
        _returnState = new EnemyReturnState(this);
        _combatEndState = new EnemyCombatEndState(this);
    }

    public Enemy Enemy => _enemy;
    public EnemyIdleState IdleState => _idleState;
    public EnemyHitState HitState => _hitState;
    public EnemyDeadState DeadState => _deadState;
    public EnemyPatrolState PatrolState => _patrolState;
    public EnemyApproachState ApproachState => _approachState;
    public EnemyCombatState CombatState => _combatState;
    public EnemyReturnState ReturnState => _returnState;
    public EnemyCombatEndState CombatEndState => _combatEndState;
    
    public EnemyReusableData ReusableData => _reusableData;
    
    public void SetHitInfo(Transform attacker, float damage, Vector3 hitDirection, bool heavy)
    {
        _reusableData.HitInfo.Attacker = attacker;
        _reusableData.HitInfo.Damage = damage;
        _reusableData.HitInfo.HitDirection = hitDirection;
        _reusableData.HitInfo.Heavy = heavy;
        _reusableData.ReciveHit = true;
    }
}
