using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerNormalAttackData 
{
    [SerializeField] private float _canAttackAfterTimeRate = 0.9f;
    [SerializeField] private float _canInputAfterTimeRate = 0.5f;
    [SerializeField] private float _canRotateBeforeTimeRate = 0.3f;
    [SerializeField] private float _rotationTime = 0.3f;
    [SerializeField] private List<float> _damage = new List<float> { 1f, 2f, 3f, 4f };
    [SerializeField] private List<bool> _heavyAttack = new List<bool> { false, false, false, true };
    

    public float CanAttackAfterTimeRate => _canAttackAfterTimeRate;
    public float CanInputAfterTimeRate => _canInputAfterTimeRate;
    public float CanRotateBeforeTimeRate => _canRotateBeforeTimeRate;
    public float RotationTime => _rotationTime;
    public List<float> Damage => _damage;
    public List<bool> HeavyAttack => _heavyAttack;
}
