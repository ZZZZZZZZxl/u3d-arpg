using System;
using UnityEngine;

[Serializable]
public class PlayerSpecialAttackData
{
    [SerializeField] private float _canAttackTimeRate = 0.3f;
    [SerializeField] private float _canInputTimeRate = 0.2f;
    [SerializeField] private float _damage = 10f;
    
    public float CanAttackTimeRate => _canAttackTimeRate;
    public float CanInputTimeRate => _canInputTimeRate;
    public float Damage => _damage;
}