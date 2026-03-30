using System;
using UnityEngine;

[Serializable]
public class PlayerSprintData
{
    [SerializeField] private float _speedModifier = 2f;
    public float SpeedModifier => _speedModifier;
    
    
}