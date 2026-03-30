using UnityEngine;

[UnityEngine.CreateAssetMenu(fileName = "AIActionData", menuName = "Create/AI/AIActionData", order = 0)]
public class AIActionData : UnityEngine.ScriptableObject
{
    [SerializeField] private float _detectionDistance = 15f; // 检测范围
    [SerializeField] private float _attackDistance = 3f; // 可以攻击的距离
    [SerializeField] private float _attackColdTime = 5f;
    [SerializeField] private EnemyIdleData _enemyIdleData ;
    [SerializeField] private EnemyPatrolData _enemyPatrolData;
    [SerializeField] private EnemyApproachData _enemyApproachData;
    [SerializeField] private EnemyCombatData _enemyCombatData;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _targetRotationReachTime = 0.14f;
    
    
    public float DetectionDistance => _detectionDistance;
    public float AttackDistance => _attackDistance;
    public float AttackColdTime => _attackColdTime;
    public EnemyIdleData EnemyIdleData => _enemyIdleData;
    public EnemyPatrolData EnemyPatrolData => _enemyPatrolData;
    public EnemyApproachData EnemyApproachData => _enemyApproachData;
    public EnemyCombatData EnemyCombatData => _enemyCombatData;
    
    public LayerMask GroundLayer => _groundLayer;
    public float TargetRotationReachTime => _targetRotationReachTime;
}