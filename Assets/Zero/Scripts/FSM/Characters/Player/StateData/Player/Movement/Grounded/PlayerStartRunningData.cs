using System;
using UnityEngine;

[Serializable]
public class PlayerStartRunningData
{
    [SerializeField]private float _timeToRunState = 1.2f;
    [SerializeField] private float _timeToIdle = 0.174f;
    public float TimeToRunState => _timeToRunState;
    public float TimeToIdle => _timeToIdle;
}