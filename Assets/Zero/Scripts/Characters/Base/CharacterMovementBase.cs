using UnityEngine;

public abstract class CharacterMovementBase : MonoBehaviour
{
    protected Animator _animator;
    protected CharacterController _controller;
    
    //参数
    protected const float GRAVITY = -9.8f;
    protected const float FALLTIME = 0.15f;
    protected float _fallOutDeltaTime;
    protected float _verticalSpeed;
    protected readonly float _maxVerticalSpeed = 60f;
    [SerializeField, Header("地面检测")] protected float _detectionCenterOffset;
    [SerializeField, Header("地面检测")] protected float _detectionRang;
    
    protected Vector3 _movementDirection;
    protected Vector3 _gravityDirection;
    
    protected bool _enableRootMotion;
    protected bool _isOnGround;
    protected bool _enableGravity;

    [SerializeField] protected LayerMask _whatIsGround;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    protected virtual void Start()
    {
        _enableGravity = true;
        _enableRootMotion = true;
        _fallOutDeltaTime = FALLTIME;
    }

    protected virtual void FixedUpdate()
    {
        IsOnGround();
    }

    protected virtual void Update()
    {
        UpdateCharacterGravity();
        CharacterVerticalMovement();
    }

    protected virtual void OnAnimatorMove()
    {
        if(!_enableRootMotion) return;
        _animator.ApplyBuiltinRootMotion();
        UpdateCharacterMovementDirection(_animator.deltaPosition);
    }

    protected void UpdateCharacterMovementDirection(Vector3 direction)
    {
        _movementDirection = SlopResetDirection(direction);
        _controller.Move(_movementDirection * Time.deltaTime);
    }
    
    //计算角色重力
    private void UpdateCharacterGravity()
    {
        if (_isOnGround)
        {
            _fallOutDeltaTime = FALLTIME; 
            _verticalSpeed = (_verticalSpeed < 0) ? -2f : _verticalSpeed;
        }
        else
        {
            if (_fallOutDeltaTime > 0f) _fallOutDeltaTime -= Time.deltaTime;
            
            if (_verticalSpeed < _maxVerticalSpeed && _enableGravity)
                _verticalSpeed += GRAVITY * Time.deltaTime;
        }
    }
    
    //角色因为重力下落
    private void CharacterVerticalMovement()
    {
        _gravityDirection.Set(0f,_verticalSpeed,0f);
        _controller.Move(_gravityDirection * Time.deltaTime);
    }
    
    //检测地面
    private void IsOnGround()
    {
        var detectionCenter = new Vector3(transform.position.x, transform.position.y - _detectionCenterOffset,
            transform.position.z);
        _isOnGround = Physics.CheckSphere(detectionCenter, _detectionRang, _whatIsGround, QueryTriggerInteraction.Ignore);
        // _animator.SetBool(AnimatorParameter.OnGroundID,_isOnGround);
    }
    
    //坡道重置移动方向(沿着坡度方向移动)
    protected Vector3 SlopResetDirection(Vector3 moveDirection)
    {
        if (Physics.Raycast(transform.position + (Vector3.up * .3f), -Vector3.up, out var _hit,
                _controller.height * 2f, _whatIsGround, QueryTriggerInteraction.Ignore))
        {
            var angle = Vector3.Dot(Vector3.up, _hit.normal);
            if (angle != 0f && _verticalSpeed<0f)
            {
                return Vector3.ProjectOnPlane(moveDirection, _hit.normal);
            }
        }
        return moveDirection;
    }
    
    //OnDrawGizmos绘制
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _isOnGround ? Color.green : Color.red;

        Vector3 position = Vector3.zero;

        position.Set(transform.position.x, transform.position.y - _detectionCenterOffset,
            transform.position.z);

        Gizmos.DrawWireSphere(position, _detectionRang);
    }
}

