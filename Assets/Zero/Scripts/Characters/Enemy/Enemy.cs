using System;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(EnemyHealthController))]
public class Enemy : MonoBehaviour
{
    
    [SerializeField] private AIActionData _aiActionData;
    [SerializeField] private Transform _weapon;
    
    private EnemyStateMachine _stateMachine;
    private EnemyHealthController _healthController;
    private Animator _animator;
    private CharacterController _characterController;
    private EnemyReusableData _reusableData;
    private Vector3 _moveDirection;
    // private NavMeshAgent _agent;

    public EnemyStateMachine EnemyStateMachine => _stateMachine;
    public EnemyHealthController HealthController => _healthController;
    public Animator Animator => _animator;
    public CharacterController Controller => _characterController;
    public EnemyReusableData ReusableData => _reusableData;
    public AIActionData Data => _aiActionData;
    public Vector3 MoveDirection => _moveDirection;
    public Transform Weapon => _weapon;

    #region Unity Methods

    private void Awake()
    {
        _reusableData = new EnemyReusableData();
        
        // _agent = GetComponent<NavMeshAgent>();
        // _agent.updatePosition = false;
        // _agent.updateRotation = false;
        
        _healthController = GetComponent<EnemyHealthController>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _stateMachine = new EnemyStateMachine(this);
    }

    private void Start()
    {
        _reusableData.OriginPosition = _stateMachine.Enemy.transform.position;
        
        _stateMachine.ChangeState(_stateMachine.IdleState);
        ReusableData.TimeToReachTargetRotation = _aiActionData.TargetRotationReachTime;
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        UpdateRotation();
        _stateMachine.PhysicsUpdate();
    }

    private void OnAnimatorMove()
    {
        _animator.ApplyBuiltinRootMotion();
        _moveDirection = _animator.deltaPosition;
    }

    public void GetMoveDirectionOfTarget()
    {
        
    }

    #endregion

    #region AI



    #endregion

    #region Animation Event

    public void OnCombatStateAnimationEnter()
    {
        _stateMachine.OnAnimationEnter();
    }

    public void OnCombatStateAnimationExit()
    {
        _stateMachine.OnAnimationExit();
    }

    public void OnCombatStateAnimationTransition()
    {
        _stateMachine.OnAnimationTransition();
    }

    public void OnCombatStateAnimationEvent()
    {
        _stateMachine.OnAnimationEvent();
    }

    #endregion
    
    #region Rotation Methods

    private void UpdateRotation()
    {
        if (_reusableData.TargetDirection != Vector2.zero)
        {
            UpdateTargetAngle();
        }
        
        // UpdateTargetAngle();
        

        // if (!_reusableData.ShouldRotate)
        // {
        //     _reusableData.DampedTargetRotationPassedTime = 0f;
        //     return;
        // }

        // UpdateTurnAround();
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
            Mathf.Atan2(_reusableData.TargetDirection.x, _reusableData.TargetDirection.y) * Mathf.Rad2Deg;
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

    // private void UpdateTurnAround()
    // {
    //     float deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.y, _reusableData.CurrentTargetRotation);
    //
    //     if (Mathf.Abs(deltaAngle) > 135f)
    //     {
    //         _animator.SetBool(AnimationID.TurnAroundID, true);
    //     }
    //     else
    //     {
    //         _animator.SetBool(AnimationID.TurnAroundID, false);
    //     }
    // }
    
    #endregion
    
}
