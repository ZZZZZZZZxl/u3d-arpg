using System;
using UnityEngine;

[Serializable]
public class PlayerRushAttackingData
{
    [SerializeField] private float canTransiteTimeRate = 0.5f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private bool heavy = true;
    
    public float CanTransiteTimeRate => canTransiteTimeRate;
    public float Damage => damage;
    public bool Heavy => heavy;
}