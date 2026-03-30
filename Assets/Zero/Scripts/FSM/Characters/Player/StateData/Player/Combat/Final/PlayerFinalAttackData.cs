using System;
using UnityEngine;

[Serializable]
public class PlayerFinalAttackData
{
    [SerializeField] private float _canTransitionTimeRate = 0.8f;
    [SerializeField] private float _canInputTimeRate = 0.67f;
    [SerializeField] private float _damage = 25f;
    
    public float CanTransitionTimeRate => _canTransitionTimeRate;
    public float CanInputTimeRate => _canInputTimeRate;
    public float Damage => _damage;
}