using GGG.Tool;
using UnityEngine;

namespace Zero.Scripts
{
    public class Debug : MonoBehaviour
    {
        [Header("全局播放速度调节")]
        [Range(0f, 2f)]
        public float timeScale = 1f;   // Inspector可调
        private float _lastTimeScale = 1f;

        public bool resetTimeScale = false;
        
        public Transform enemy;
        public float attackDistance;
        public Transform player;
        public float distanceToPlayer;
        
        
        private void Update()
        {
            UpdateTimeScale();
            ShowInfo();
        }

        private void OnDisable()
        {
            // 脚本被禁用时恢复正常时间
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        private void ShowInfo()
        {
            var temp = enemy.GetComponent<Enemy>();
            // attackDistance = temp.ReusableData.AttackDistance;
            // distanceToPlayer = DevelopmentToos.DistanceForTarget(enemy, player);
        }
        
        private void UpdateTimeScale()
        {
            if (resetTimeScale)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            }
            
            // 只有修改时才更新，避免重复赋值
            if (!Mathf.Approximately(_lastTimeScale, timeScale))
            {
                _lastTimeScale = timeScale;
            
                // 修改全局时间
                Time.timeScale = timeScale;
                Time.fixedDeltaTime = 0.02f * timeScale; // 保证物理稳定
            }
        }
    }
}