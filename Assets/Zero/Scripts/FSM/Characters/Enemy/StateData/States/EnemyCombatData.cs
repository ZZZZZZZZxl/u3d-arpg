using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackDistanceRow
{
    public List<float> values;
}

[Serializable]
public class EnemyCombatData
{
    
    [SerializeField] private List<float> _damage = new List<float> { 1f, 2f, 3f};
    [SerializeField] private List<bool> _heavyAttack = new List<bool> { false, false, true };
    [SerializeField] private float _canAttackAfterTimeRate = .9f;
    [SerializeField] private static List<AttackDistanceRow> _attackDistance = new()
    {
        new AttackDistanceRow { values = new List<float> { 3f } },
        new AttackDistanceRow { values = new List<float> { 3f } },
        new AttackDistanceRow { values = new List<float> { 5f, 10f } },
    };
    
    public float CanAttackAfterTimeRate => _canAttackAfterTimeRate;
    public List<AttackDistanceRow> AttackDistance = _attackDistance;
    public List<float> Damage => _damage;
    public List<bool> HeavyAttack => _heavyAttack;
}