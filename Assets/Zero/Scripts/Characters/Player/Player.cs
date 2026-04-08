using System;
using GGG.Tool;
using UnityEngine;

public class Player : CharacterMovementBase
{
    [SerializeField] private PlayerSO data;
    [SerializeField] private Transform weaponBase;
    [SerializeField] private Transform weaponTop;
    private PlayerMovementStateMachine _movementStateMachine;
    private PlayerCombatStateMachine _combatStateMachine;
    private PlayerStateReusableData _reusableData;
    private Transform _mainCamera;
    
    public Transform WeaponBase => weaponBase;
    public Transform WeaponTop => weaponTop;
    public PlayerSO Data => data;
    public Animator Animator => _animator;
    public PlayerMovementStateMachine MovementStateMachine => _movementStateMachine;
    public PlayerCombatStateMachine CombatStateMachine => _combatStateMachine;
    public PlayerStateReusableData ReusableData => _reusableData;
    
    protected override void Awake()
    {
        base.Awake();
        if (Camera.main != null) _mainCamera = Camera.main.transform;
        _animator =  GetComponent<Animator>();
        _reusableData = new PlayerStateReusableData();
        _movementStateMachine = new PlayerMovementStateMachine(this);
        _combatStateMachine = new PlayerCombatStateMachine(this);
    }


    protected override void Start()
    {
        base.Start();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _animator.SetBool(AnimationID.EndDashID, true);
        _movementStateMachine.ChangeState(_movementStateMachine.PlayerIdlingState);

        ReusableData.TimeToReachTargetRotation = data.PlayerRotationData.TargetRotationReachTime;
    }

    protected override void Update()
    {
        base.Update();
        
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        UpdateRotation();
        _movementStateMachine.PhysicsUpdate();
        _combatStateMachine.PhysicsUpdate();
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
