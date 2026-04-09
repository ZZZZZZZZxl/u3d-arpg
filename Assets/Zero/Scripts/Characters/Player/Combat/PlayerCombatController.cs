using System;
using GGG.Tool;
using UnityEngine;


public class PlayerCombatController : MonoBehaviour
{
    private Player _player;
    public Transform CurrentEnemy => _player.ReusableData.CurrentEnemy;
    
    private void OnEnable()
    {
        _player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        ClearEnemy();
    }

    private void FixedUpdate()
    {
        EnemyDetection();
    }


    #region Enemy Detection

    protected void EnemyDetection()
    {
        GetDetectionDiretion();
        
        if (!_player.ReusableData.CanChangeEnemy)
            return ;

        
        Vector3 origin = transform.position + transform.up * 0.7f;
        origin += _player.ReusableData.DetectionDirectrion * _player.Data.CombatData.DetectionRadius; 
        
        RaycastHit[] hits = 
            Physics.SphereCastAll( 
                origin, 
                _player.Data.CombatData.DetectionRadius, 
                _player.ReusableData.DetectionDirectrion,
                _player.Data.CombatData.MaxDetectionDistance, 
                _player.Data.CombatData.EnemyLayerMask, 
                QueryTriggerInteraction.Ignore
            ); 
        
        if (hits.Length == 0) return; 
        
        var bestTarget = GetBestTarget(hits);
        _player.ReusableData.CurrentEnemy = bestTarget;     
    }

    private Transform GetBestTarget(RaycastHit[] hits)
    {
        Transform bestTarget = null; 
        int bestAngleBucket = int.MaxValue; 
        float bestDistance = float.MaxValue; 
        Vector3 selfPos = transform.position; 
        Vector3 dirNorm = _player.ReusableData.DetectionDirectrion.normalized; 
        
        foreach (var hit in hits) 
        { 
            Vector3 toTarget = hit.transform.position - selfPos; 
            float distance = toTarget.magnitude; 
            if (distance <= 0.001f) continue;
            Vector3 toTargetDir = toTarget / distance; 
            
            // 夹角（0~180）
            float angle = Vector3.Angle(dirNorm, toTargetDir); 
            
            // 每 5° 一个箱
            int angleBucket = Mathf.FloorToInt(angle / 5f); 
            
            // 角度箱更小，直接替换
            if (angleBucket < bestAngleBucket)
            {
                bestAngleBucket = angleBucket; 
                bestDistance = distance; 
                bestTarget = hit.collider.transform;
            } // 同角度箱，距离更近
            else if (angleBucket == bestAngleBucket && distance < bestDistance)
            {
                bestDistance = distance; 
                bestTarget = hit.collider.transform;
            } 
        }

        return bestTarget;
    }


    private void GetDetectionDiretion()
    {
        if (_player.ReusableData.MovementInput != Vector2.zero)
            _player.ReusableData.DetectionDirectrion =
                _player.ReusableData.MovementInput.y * ObjectsManager.MainInstance.MainCamera.forward
                + _player.ReusableData.MovementInput.x * ObjectsManager.MainInstance.MainCamera.right;
        else
            _player.ReusableData.DetectionDirectrion = ObjectsManager.MainInstance.MainCamera.forward +
                                                       ObjectsManager.MainInstance.MainCamera.right;
        
        _player.ReusableData.DetectionDirectrion.y = 0;
        _player.ReusableData.DetectionDirectrion = _player.ReusableData.DetectionDirectrion.normalized;
    }

    #endregion


    #region ClearEnemy

    private void  ClearEnemy()
    {
        if (_player.ReusableData.CurrentEnemy == null)
            return;
        
        if (!_player.ReusableData.CanChangeEnemy)
            return;
        
        if (DevelopmentToos.DistanceForTarget(_player.ReusableData.CurrentEnemy, transform) > _player.Data.CombatData.MaxDetectionDistance)
        {
            _player.ReusableData.CurrentEnemy = null;
            return;
        }
        
        // 正在移动，且移动的方向里没有当前目标
        if (DevelopmentToos.GetAngleForTargetDirection(_player.ReusableData.CurrentEnemy, transform) > 80f)
        {
            _player.ReusableData.CurrentEnemy = null;
        }
    }

    #endregion
    
}