using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Create/Combat/CombatData", order = 0)]
public class CombatData : ScriptableObject
{
    [SerializeField] private PlayerNormalAttackData _playerNormalAttackData;
    [SerializeField] private PlayerSpecialAttackData _specialAttackData;
    [SerializeField] private PlayerFinalAttackData _finalAttackData;
    [SerializeField] private PlayerRushAttackingData _rushAttackingData;
    [SerializeField] private LayerMask _enemyLayerMask;   
    [SerializeField] private float _detectionRadius = 0.5f;
    [SerializeField] private float _maxDetectionDistance = 8f;
    [SerializeField] private float _frontAreaAngle = 270f;
    [SerializeField] protected float _attackDistance = 8f;
    
    public PlayerNormalAttackData PlayerNormalAttackData => _playerNormalAttackData;
    public PlayerSpecialAttackData SpecialAttackData => _specialAttackData;
    public PlayerFinalAttackData FinalAttackData => _finalAttackData;
    public PlayerRushAttackingData RushAttackingData => _rushAttackingData;
    public LayerMask EnemyLayerMask => _enemyLayerMask;
    public float DetectionRadius => _detectionRadius;
    public float MaxDetectionDistance => _maxDetectionDistance;
    public float FrontAreaAngle => _frontAreaAngle;
    public float AttackDistance => _attackDistance;
}