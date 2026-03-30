using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNormalAttackingState : PlayerAttackingState
{
    private bool _hasInputInThisAttack; // 用于判断这个攻击有没有输入下一个攻击的指令
    
    private float _canAttackAfterTimeRate;
    private float _canInputAfterTimeRate;
    private float _rotationTime;
    private float _rotationPassTime;
    
    private int _attackIndex;
    private List<float> _damageList;
    private float _attackDistance;
    
    public PlayerNormalAttackingState(PlayerCombatStateMachine stateMachine) : base(stateMachine)
    {
        _canAttackAfterTimeRate = _combatData.PlayerNormalAttackData.CanAttackAfterTimeRate;
        _canInputAfterTimeRate = _combatData.PlayerNormalAttackData.CanInputAfterTimeRate;
        _rotationTime = _combatData.PlayerNormalAttackData.RotationTime;
        _damageList = _combatData.PlayerNormalAttackData.Damage;
        DevelopmentToos.WTF(_attackDistance);
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        _attackIndex = -1;
        
        UpdateCurrentAttack();
        
        _stateMachine.ReusableData.ShouldRotate = false;
        _stateMachine.ReusableData.CanAttack = true;
        _rotationPassTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        
        UpdateRotation();
        UpdateAttackInfo();
        
    }

    public override void OnAnimationEvent()
    {
        _stateMachine.ReusableData.CanAttack = true;
    }

    public override void OnAnimationTransition()
    {
        _stateMachine.ChangeState(_stateMachine.PlayerNormalAttackEndState);
    }

    #endregion
    
    
    #region Main Methods
    
    private void UpdateRotation()
    {
        if (!CheckRotation()) return;

        // _stateMachine.Player.transform.rotation = Quaternion.Euler(0, _stateMachine.ReusableData.CurrentTargetRotation, 0);
        
        float smoothTime = Mathf.Max(0f, _rotationTime - _rotationPassTime);
        float rotationY = Mathf.SmoothDampAngle(_stateMachine.Player.transform.eulerAngles.y, _stateMachine.ReusableData.CurrentTargetRotation, 
                ref _stateMachine.ReusableData.DampedTargetRotationVelocity, smoothTime);
        
        _rotationPassTime += Time.deltaTime;
        
        if (Mathf.Abs(Mathf.DeltaAngle(rotationY, _stateMachine.ReusableData.CurrentTargetRotation)) < 2f)
        {
            _stateMachine.Player.transform.rotation = Quaternion.Euler(0, _stateMachine.ReusableData.CurrentTargetRotation, 0);
            _rotationPassTime = 0f;
            return;
        }
        
        _stateMachine.Player.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    private bool CheckRotation()
    {
        if (!CheckAnimation()) return false;

        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >
            _combatData.PlayerNormalAttackData.CanRotateBeforeTimeRate)
        {
            return false;
        }

        return true;
    }

    private void UpdateCurrentAttack()
    {
        _hasInputInThisAttack = false;
        _attackIndex++;
        
        if (_attackIndex == _damageList.Count)
            _attackIndex = 0;


        _stateMachine.ReusableData.Damage = _damageList[_attackIndex];
        _stateMachine.ReusableData.Heavy = _combatData.PlayerNormalAttackData.HeavyAttack[_attackIndex];
        _stateMachine.ReusableData.CanAttack = false;
    }

    
    private bool CheckAnimation()
    {
        if (_stateMachine.Player.Animator.IsInTransition(0))
            return false;
        if (!_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).IsTag("NormalAttack"))
            return false;
        return true;
    }

    private void UpdateAttackInfo()
    {
        if (!CheckAnimation())
            return;
        
        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < _canAttackAfterTimeRate) 
            return;

        _stateMachine.ReusableData.CanAttack = true;
        
        if (_hasInputInThisAttack) // 攻击执行完之后 有输入无需切换状态 重新计时下一个
        {
            _stateMachine.Player.Animator.SetTrigger(AnimationID.TransiteNextAttackID);
            _stateMachine.Player.Animator.SetBool(AnimationID.InNormalAttackID, true); // 在normal状态里面替换状态
            UpdateCurrentAttack();
        }
    }

    private bool CheckDistanceNeedModify()
    {
        if (!_stateMachine.ReusableData.CurrentEnemy)
            return false;
        
        float distance =
            DevelopmentToos.DistanceForTarget(_stateMachine.ReusableData.CurrentEnemy, _stateMachine.Player.transform);

        if (distance > _attackDistance && distance < _attackDistance + 1f)
            return true;

        return false;
    }

    #endregion
    

    #region Reusable Methods

    protected override void StartDamage()
    {
        base.StartDamage();
        
        if (!CheckDistanceNeedModify())
            return;

        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f)
            return;
        
        ExcuteModify(_stateMachine.ReusableData.CurrentEnemy.position +
                     (-_stateMachine.Player.transform.forward * _attackDistance));
    }

    protected override void SetEnterAnimationParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.AttackID, true);
    }

    protected override void SetExitAnimationParameters()
    {
        _stateMachine.Player.Animator.SetBool(AnimationID.AttackID, false);
        _stateMachine.Player.Animator.SetBool(AnimationID.InNormalAttackID, false);
    }

    #endregion
    

    #region InputActions

    protected override void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (_stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _canInputAfterTimeRate)
            _hasInputInThisAttack = true;
    }   

    #endregion
}
