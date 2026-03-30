using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Enemy _enemy;
    private Player _player;
    private void Awake()
    {
        TryGetComponent<Player>(out _player);
        TryGetComponent<Enemy>(out _enemy);
    }


    #region Player

    public void TriggerOnMovementStateAnimationEnter()
    {
        if (IsInPlayerAnimatorTransition())
            return;
        _player?.OnMovementStateAnimationEnter();
    }

    public void TriggerOnMovementStateAnimationExit()
    {
        if (IsInPlayerAnimatorTransition())
            return;
        _player?.OnMovementStateAnimationExit();
    }

    public void TriggerOnMovementStateAnimationTransition()
    {
        if (IsInPlayerAnimatorTransition())
        {
            return;
        }
        _player?.OnMovementStateAnimationTransition();
    }
    
    public void TriggerOnMovementStateAnimationEvent()
    {
        if (IsInPlayerAnimatorTransition())
            return;
        _player?.OnMovementStateAnimationEvent();
    }
    
    
    
    public void TriggerOnCombatStateAnimationEnter()
    {
        if (_player)
        {
            if (IsInPlayerAnimatorTransition())
                return;

            _player?.OnCombatStateAnimationEnter();
            return;
        }

        if (IsInEnemyAnimatorTransition())
            return;
        
        _enemy?.OnCombatStateAnimationEnter();
    }

    public void TriggerOnCombatStateAnimationExit()
    {
        if (_player)
        {
            if (IsInPlayerAnimatorTransition())
                return;

            _player?.OnCombatStateAnimationExit();
        }
        
        if (IsInEnemyAnimatorTransition()) 
            return;
        _enemy?.OnCombatStateAnimationExit();
    }

    public void TriggerOnCombatStateAnimationTransition()
    {
        if (_player)
        {
            if (IsInPlayerAnimatorTransition())
                return;

            _player?.OnCombatStateAnimationTransition();
            return;
        }
        if (IsInEnemyAnimatorTransition()) 
            return;
        _enemy?.OnCombatStateAnimationTransition();
    }
    
    public void TriggerOnCombatStateAnimationEvent()
    {
        if (_player)
        {
            if (IsInPlayerAnimatorTransition())
                return;

            _player?.OnCombatStateAnimationEvent();
            return;
        }

        if (IsInEnemyAnimatorTransition())
            return;
        _enemy?.OnCombatStateAnimationEvent();
    }

    private bool IsInPlayerAnimatorTransition(int layer = 0)
    {
        if (!_player) return false;
        return _player.Animator.IsInTransition(layer);
    }

    #endregion


    #region Enemy

    
    
    private bool IsInEnemyAnimatorTransition(int layer = 0)
    {
        if (!_enemy) return false;
        return _enemy.Animator.IsInTransition(layer);
    }

    #endregion
    
}
