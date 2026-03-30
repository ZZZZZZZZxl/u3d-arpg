using System;
using GGG.Tool;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerSO data;
    [SerializeField] private Transform weaponBase;
    [SerializeField] private Transform weaponTop;
    private PlayerMovementStateMachine _movementStateMachine;
    private PlayerCombatStateMachine _combatStateMachine;
    private PlayerStateReusableData _reusableData;
    private CharacterController _controller;
    private Transform _mainCamera;
    private Animator _animator;

    private Vector3 _moveDirection;
    
    public CharacterController Controller => _controller;

    public Transform WeaponBase => weaponBase;
    public Transform WeaponTop => weaponTop;
    public PlayerSO Data => data;
    public Animator Animator => _animator;
    public Vector3 MoveDirection => _moveDirection;
    public PlayerMovementStateMachine MovementStateMachine => _movementStateMachine;
    public PlayerCombatStateMachine CombatStateMachine => _combatStateMachine;
    public PlayerStateReusableData ReusableData => _reusableData;
    
    private void Awake()
    {
        _mainCamera = Camera.main.transform;
        _controller = GetComponent<CharacterController>();
        _animator =  GetComponent<Animator>();
        _reusableData = new PlayerStateReusableData();
        _movementStateMachine = new PlayerMovementStateMachine(this);
        _combatStateMachine = new PlayerCombatStateMachine(this);
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _animator.SetBool(AnimationID.EndDashID, true);
        _movementStateMachine.ChangeState(_movementStateMachine.PlayerIdlingState);

        ReusableData.TimeToReachTargetRotation = data.PlayerRotationData.TargetRotationReachTime;
    }

    private void Update()
    {
        
        _animator.SetFloat(
            AnimationID.MovementId, 
            _reusableData.MovementInput.SqrMagnitude() * _reusableData.MovementSpeedModifier, 
            data.GroundedData.MoveDampTime, 
            Time.deltaTime
        );
        
        _movementStateMachine.HandleInput();
        _movementStateMachine.Update();
        
        _combatStateMachine.HandleInput();
        _combatStateMachine.Update();
    }

    private void FixedUpdate()
    {
        UpdateRotation();
        _movementStateMachine.PhysicsUpdate();
        _combatStateMachine.PhysicsUpdate();
    }

    public void OnAnimatorMove()
    {
        _animator.ApplyBuiltinRootMotion();
        _moveDirection = _animator.deltaPosition;
    }
    
    private void OnDrawGizmos()
    {
        if (WeaponBase == null || WeaponTop == null) return;

        Vector3 start = WeaponBase.position + (WeaponTop.position - WeaponBase.position) / 3;
        Vector3 end = WeaponTop.position;
        float radius = 0.1f;

        Gizmos.color = Color.red;

        // 两端的圆
        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);

        // 简单画侧边线表示圆柱
        Vector3 dir = (end - start).normalized;
        Vector3 cross = Vector3.Cross(dir, Vector3.up);
        if (cross == Vector3.zero)
            cross = Vector3.Cross(dir, Vector3.right);
        cross = cross.normalized * radius;

        Vector3 offset1 = cross;
        Vector3 offset2 = -cross;
        Vector3 offset3 = Vector3.Cross(dir, cross).normalized * radius;
        Vector3 offset4 = -offset3;

        Gizmos.DrawLine(start + offset1, end + offset1);
        Gizmos.DrawLine(start + offset2, end + offset2);
        Gizmos.DrawLine(start + offset3, end + offset3);
        Gizmos.DrawLine(start + offset4, end + offset4);
    }

    #region RecevieHit

    public void SetHitInfo(Transform attacker, float damage, Vector3 hitDirection, bool heavy)
    {
        _reusableData.HitInfo.Attacker = attacker;
        _reusableData.HitInfo.Damage = damage;
        _reusableData.HitInfo.HitDirection = hitDirection;
        _reusableData.HitInfo.Heavy = heavy;
        _reusableData.ReciveHit = true;
    }

    #endregion


    #region Rotation Methods

    private void UpdateRotation()
    {
        if (_reusableData.MovementInput != Vector2.zero)
        {
            UpdateTargetAngle();
        }
        

        if (!_reusableData.ShouldRotate)
        {
            _reusableData.DampedTargetRotationPassedTime = 0f;
            return;
        }

        UpdateTurnAround();
        Rotation();
    }

    private void Rotation()
    {
        var rotationY = GetRotationY();

        CharacterRotateByAngle(rotationY);
    }

    private void UpdateTargetAngle()
    {
        var targetAngle = GetTargetAngleByDirection();

        if (Mathf.Abs(_reusableData.CurrentTargetRotation - targetAngle) > 0.001f)
        {
            UpdateRotationData(targetAngle);
        }
    }

    private float GetTargetAngleByDirection()
    {
        float targetAngle =
            Mathf.Atan2(_reusableData.MovementInput.x, _reusableData.MovementInput.y) * Mathf.Rad2Deg
            + _mainCamera.eulerAngles.y;
        return targetAngle;
    }
    
    private void UpdateRotationData(float targetAngle)
    {
        _reusableData.CurrentTargetRotation = targetAngle;
        _reusableData.DampedTargetRotationPassedTime = 0f;
    }

    private float GetRotationY()
    {
        float smoothTime = Mathf.Max(
            0f,
            _reusableData.TimeToReachTargetRotation -
            _reusableData.DampedTargetRotationPassedTime
        );

        float rotationY =
            Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                _reusableData.CurrentTargetRotation,
                ref _reusableData.DampedTargetRotationVelocity,
                smoothTime
            );

        _reusableData.DampedTargetRotationPassedTime += Time.deltaTime;
        return rotationY;
    }

    private void CharacterRotateByAngle(float rotationEulerAngleY)
    {
        transform.rotation = Quaternion.Euler(0f, rotationEulerAngleY, 0f);
    }

    private void UpdateTurnAround()
    {
        float deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.y, _reusableData.CurrentTargetRotation);

        if (Mathf.Abs(deltaAngle) > 135f)
        {
            _animator.SetBool(AnimationID.TurnAroundID, true);
        }
        else
        {
            _animator.SetBool(AnimationID.TurnAroundID, false);
        }
    }
    
    #endregion


    #region Animation Events

    
    public void OnMovementStateAnimationEnter()
    {
        _movementStateMachine.OnAnimationEnter();
    }
    
    public void OnMovementStateAnimationExit()
    {
        _movementStateMachine.OnAnimationExit();
    }
    
    public void OnMovementStateAnimationTransition()
    {
        _movementStateMachine.OnAnimationTransition();
    }

    public void OnMovementStateAnimationEvent()
    {
        _movementStateMachine.OnAnimationEvent();
    }
    
    public void OnCombatStateAnimationEnter()
    {
        _combatStateMachine.OnAnimationEnter();
    }

    public void OnCombatStateAnimationExit()
    {
        _combatStateMachine.OnAnimationExit();
    }

    public void OnCombatStateAnimationTransition()
    {
        _combatStateMachine.OnAnimationTransition();
    }
    
    public void OnCombatStateAnimationEvent()
    {
        _combatStateMachine.OnAnimationEvent();
    }

    #endregion
    
}