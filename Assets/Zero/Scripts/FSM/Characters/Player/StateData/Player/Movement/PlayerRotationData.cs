using System;
using UnityEngine;

[Serializable]
public class PlayerRotationData
{
    [SerializeField] private float _targetRotationReachTime = 0.14f;
    
    public  float TargetRotationReachTime => _targetRotationReachTime;
}