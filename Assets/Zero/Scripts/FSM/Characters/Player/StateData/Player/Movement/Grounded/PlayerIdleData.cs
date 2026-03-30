using System;
using UnityEngine;

[Serializable]
public class PlayerIdleData
{
    [SerializeField] private float _speedModifier = 0f;
    public float SpeedModifier => _speedModifier;
}