using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(EnemyHealthController))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : CharacterMovementBase
{
    
    [SerializeField] private AIActionData _aiActionData;
    [SerializeField] private Transform _weapon;
    
    private EnemyStateMachine _stateMachine;
    private EnemyHealthController _healthController;
    private EnemyReusableData _reusableData;
    private Vector3 _moveDirection;
    private NavMeshAgent _agent;

    public EnemyStateMachine EnemyStateMachine => _stateMachine;
    public EnemyHealthController HealthController => _healthController;
    public Animator Animator => _animator;
    public EnemyReusableData ReusableData => _reusableData;
    public AIActionData Data => _aiActionData;
    public Vector3 MoveDirection => _moveDirection;
    public Transform Weapon => _weapon;
    public CharacterController Controller => _controller;
    public NavMeshAgent Agent => _agent;

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();
        _reusableData = new EnemyReusableData();
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        
        _healthController = GetComponent<EnemyHealthController>();
        _animator = GetComponent<Animator>();
        _stateMachine = new EnemyStateMachine(this);
    }

    protected override void Start()
    {
        base.Start();
        
        _reusableData.OriginPosition = _stateMachine.Enemy.transform.position;
        
        _stateMachine.ChangeState(_stateMachine.IdleState);
        ReusableData.TimeToReachTargetRotation = _aiActionData.TargetRotationReachTime;
        SyncAgentPosition(true);
    }

    protected override void Update()
    {
        base.Update();
        
        _stateMachine.HandleInput();
        _stateMachine.Update();
        SyncAgentPosition();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        UpdateRotation();
        _stateMachine.PhysicsUpdate();
        SyncAgentPosition();
    }

    #endregion

    #region AI

    public bool SetDestination(Vector3 destination)
    {
        if (!EnsureAgentOnNavMesh())
            return false;

        _agent.isStopped = false;
        return _agent.SetDestination(destination);
    }

    public Vector2 GetSteeringDirection()
    {
        if (!EnsureAgentOnNavMesh() || _agent.pathPending || !_agent.hasPath)
            return Vector2.zero;

        Vector3 offset = _agent.steeringTarget - transform.position;
        offset.y = 0f;

        if (offset.sqrMagnitude < 0.0001f)
            return Vector2.zero;

        return new Vector2(offset.x, offset.z).normalized;
    }

    public bool HasReachedDestination(float threshold = 0.35f)
    {
        if (!EnsureAgentOnNavMesh())
            return true;

        if (_agent.pathPending)
            return false;

        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
            return true;

        float stoppingDistance = Mathf.Max(_agent.stoppingDistance, threshold);

        if (_agent.hasPath && _agent.remainingDistance > stoppingDistance)
            return false;

        Vector3 offset = _agent.destination - transform.position;
        offset.y = 0f;
        return offset.sqrMagnitude <= threshold * threshold;
    }

    public void StopAgent()
    {
        if (_agent == null || !_agent.enabled || !_agent.isOnNavMesh)
            return;

        _agent.isStopped = true;

        if (_agent.hasPath)
            _agent.ResetPath();
    }

    public void SyncAgentPosition(bool forceWarp = false)
    {
        if (_agent == null || !_agent.enabled)
            return;

        if (!EnsureAgentOnNavMesh(4f))
            return;

        if (forceWarp || (_agent.nextPosition - transform.position).sqrMagnitude > 1f)
        {
            _agent.Warp(transform.position);
            return;
        }

        _agent.nextPosition = transform.position;
    }

    private bool EnsureAgentOnNavMesh(float sampleDistance = 2f)
    {
        if (_agent == null || !_agent.enabled)
            return false;

        if (_agent.isOnNavMesh)
            return true;

        if (!NavMesh.SamplePosition(transform.position, out var hit, sampleDistance, NavMesh.AllAreas))
            return false;

        return _agent.Warp(hit.position);
    }


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
