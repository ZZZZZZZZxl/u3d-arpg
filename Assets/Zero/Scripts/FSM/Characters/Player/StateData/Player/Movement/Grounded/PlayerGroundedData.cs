using System;
using UnityEngine;

[Serializable]
public class PlayerGroundedData
{ 
    [SerializeField] private float _moveDampTime = 0.25f;
    [SerializeField] private LayerMask _groundLayer;
    
    public float MoveDampTime => _moveDampTime;
    public LayerMask GroundLayer => _groundLayer;
    
    
    [SerializeField] private PlayerRunData _playerRunData;
    [SerializeField] private PlayerSprintData _playerSprintData;
    [SerializeField] private PlayerStartRunningData _playerStartRunningData;
    
    public PlayerRunData PlayerRunData => _playerRunData;
    public PlayerSprintData PlayerSprintData => _playerSprintData;
    public PlayerStartRunningData PlayerStartRunningData => _playerStartRunningData;
    
    
    
    // [SerializeField] private PlayerIdleData _playerIdleData;
    // [SerializeField] private PlayerDashData _playerDashData;
    // public PlayerDashData PlayerDashData => _playerDashData;
    // public PlayerIdleData PlayerIdleData => _playerIdleData;
}