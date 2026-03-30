using System;
using UnityEngine;

[Serializable]
public class EnemyIdleData
{
    [SerializeField] private float _timeToPatrol = 5f; // 多久会到巡逻状态
    public float TimeToPatrol => _timeToPatrol;
}