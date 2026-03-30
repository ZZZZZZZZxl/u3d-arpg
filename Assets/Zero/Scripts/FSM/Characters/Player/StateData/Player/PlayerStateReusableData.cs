using UnityEngine;

public class PlayerStateReusableData
{
    // 运动相关
    private Vector2 _movementInput;
    private float _movementSpeedModifier;
    
    
    // 旋转相关
    private float _currentTargetRotation;
    private float _timeToReachTargetRotation;
    private float _dampedTargetRotationVelocity;
    private float _dampedTargetRotationPassedTime;
    
    private bool _shouldRotate = true;
    
    // 战斗相关
    private float _attackDistance; // 攻击范围
    private Transform _currentEnemy; // 当前目标敌人
    private bool _canChangeEnemy = true; // 是否可以切换目标敌人
    private Vector3 _detectionDirectrion; // 探测方向
    private bool _heavy;
    private float _damage;
    private HitInfo _hitInfo = new HitInfo();
    private bool _reciveHit;
    private bool _canAttack;

    public void ResetData()
    {
        _movementSpeedModifier = default;

        // 旋转相关
        // _currentTargetRotation = default;
        // _timeToReachTargetRotation = default;
        // _dampedTargetRotationVelocity = default;
        // _dampedTargetRotationPassedTime = default;
        // _shouldRotate = false; // 保持原始默认值
        // _targetDirection = default;
        // _targetDeltaAngle = default;
    }
    
    public ref Vector2  MovementInput => ref _movementInput;
    public ref float MovementSpeedModifier => ref _movementSpeedModifier;
    public ref float CurrentTargetRotation => ref _currentTargetRotation;
    public ref float TimeToReachTargetRotation => ref _timeToReachTargetRotation;
    public ref float DampedTargetRotationVelocity => ref _dampedTargetRotationVelocity;
    public ref float DampedTargetRotationPassedTime => ref _dampedTargetRotationPassedTime;
    public ref bool ShouldRotate => ref _shouldRotate;
    public ref float AttackDistance => ref _attackDistance;
    public ref Transform CurrentEnemy => ref _currentEnemy;
    public ref bool CanChangeEnemy => ref _canChangeEnemy;
    public ref Vector3 DetectionDirectrion => ref _detectionDirectrion;
    public ref bool Heavy => ref _heavy;
    public ref float Damage => ref _damage;
    public ref HitInfo HitInfo => ref _hitInfo;
    public ref bool ReciveHit => ref _reciveHit;
    public ref bool CanAttack => ref _canAttack;
}