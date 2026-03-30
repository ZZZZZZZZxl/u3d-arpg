using System;
using UnityEngine;

[Serializable]
public class PlayerRunData
{
    [SerializeField] private float _speedModifier = 1f;
    [SerializeField] private float _runToSprintTime = 2f;
    public float SpeedModifier => _speedModifier;
    public float RunToSprintTime => _runToSprintTime;  
}