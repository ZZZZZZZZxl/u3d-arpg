using System;
using UnityEngine;

public class EnemyReusableData
{
    // move direction
    private Vector3 _moveDirection;
    
    // hit
    private HitInfo _hitInfo = new HitInfo();
    private bool _reciveHit;

    // origin position
    private Vector3 _originPosition;
    
    // attack
    private bool _attackCommand;
    private Transform _targetPlayer;
    private float _lastAttackTime;

    private float _attackDistance;
    
    // rotation
    private float _currentTargetRotation;
    private float _timeToReachTargetRotation;
    private float _dampedTargetRotationVelocity;
    private float _dampedTargetRotationPassedTime;
    private Vector2 _targetDirection;
    
    public ref Vector3 MoveDirection => ref _moveDirection;
    public ref HitInfo HitInfo => ref _hitInfo;
    public ref bool ReciveHit => ref _reciveHit;
    public ref bool AttackCommand => ref _attackCommand;
    public ref Transform TargetPlayer => ref _targetPlayer;
    public ref float LastAttackTime => ref _lastAttackTime;
    public ref float AttackDistance => ref _attackDistance;
    public ref Vector3 OriginPosition => ref _originPosition;
    public ref float CurrentTargetRotation => ref _currentTargetRotation;
    public ref float TimeToReachTargetRotation => ref _timeToReachTargetRotation;
    public ref float DampedTargetRotationVelocity => ref _dampedTargetRotationVelocity;
    public ref float DampedTargetRotationPassedTime => ref _dampedTargetRotationPassedTime;
    public ref Vector2 TargetDirection => ref _targetDirection;
}