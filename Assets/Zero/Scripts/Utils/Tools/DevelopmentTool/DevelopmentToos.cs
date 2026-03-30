using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGG.Tool
{
    public class DevelopmentToos
    {

        /// <summary>
        /// 不受帧数影响的Lerp
        /// </summary>
        /// <param name="time">平滑时间(尽量设置为大于10的值)</param>
        public static float UnTetheredLerp(float time = 10f)
        {
            return 1 - Mathf.Exp(-time * Time.deltaTime);
        }

        /// <summary>
        /// 对方位于自己的方向
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="self">自身</param>
        /// <returns></returns>
        public static Vector3 DirectionForTarget(Transform self, Transform target)
        {
            return (target.position - self.position).normalized;
        }
        
        /// <summary>
        /// 对方位于自己的方向
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="self">自身</param>
        /// <returns></returns>
        public static Vector3 DirectionForTarget(Transform self, Vector3 target)
        {
            return (target - self.position).normalized;
        }

        /// <summary>
        /// 返回于目标之间的距离
        /// </summary>
        /// <param name="target"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float DistanceForTarget(Transform target, Transform self)
        {
            return Vector3.Distance(self.position, target.position);
        }
        
        public static float DistanceForTarget(Transform target, Vector3 self)
        {
            return Vector3.Distance(self, target.position);
        }

        /// <summary>
        /// 获取增量角
        /// </summary>
        /// <param name="currentDirection">当前移动方向</param>
        /// <param name="targetDirection">目标移动方向</param>
        /// <returns></returns>
        public static float GetDeltaAngle(Transform currentDirection, Vector3 targetDirection)
        {
            //当前角色朝向的角度
            float angleCurrent = Mathf.Atan2(currentDirection.forward.x, currentDirection.forward.z) * Mathf.Rad2Deg;
            //目标方向的角度也就是希望角色转过去的那个方向的角度
            float targetAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;

            return Mathf.DeltaAngle(angleCurrent, targetAngle);
        }

        /// <summary>
        /// 计算当前朝向于目标方向之间的夹角
        /// </summary>
        /// <param name="target"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float GetAngleForTargetDirection(Transform target, Transform self)
        {
            Vector3 dirToTarget = (target.position - self.position).normalized;
            return Vector3.Angle(self.forward, dirToTarget);
        }


        /// <summary>
        /// 限制一个值或者度数在-360-360之间
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float ClampValueOn360(float f)
        {
            f %= 360f;
            if (f < 0)
                f += 360;

            return f;
        }

        /// <summary>
        /// 限制一个值或者度数在-180-180之间
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float ClampValueOn180(float f)
        {
            f = (f + 180f) % 360f - 180f;

            if (f < -180)
                f += 360;

            return f;
        }

        /// <summary>
        /// 从当前位置移动到目标位置
        /// 计算当前点和目标点之间的位置，移动不超过maxDistanceDelta指定的距离。
        /// </summary>
        /// <param name="target"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Vector3 TargetPositionOffset(Transform target, Transform self, float time)
        {
            var pos = target.transform.position;
            return Vector3.MoveTowards(self.position, pos, UnTetheredLerp(time));
        }

        /// <summary>
        ///  判断目标是否在我前方多少度内
        /// </summary>
        /// <param name="target"></param>
        /// <param name="self"></param>
        /// <param name="deviationAngle"></param>
        /// <returns></returns>
        public static bool IsTargetAtFront(
            Transform target,
            Transform self,
            float deviationAngle = 120.0f   
        )
        {
            var dirToTarget = target.position - self.position;
            dirToTarget.y = 0f;
            var forward = self.forward;
            forward.y = 0f;

            dirToTarget.Normalize();
            forward.Normalize();

            var dot = Vector3.Dot(forward, dirToTarget);

            var cosAngle = Mathf.Cos(( deviationAngle / 2) * Mathf.Deg2Rad);

            return dot >= cosAngle;
        }



        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="message"></param>
        public static void WTF(object message)
        {
            Debug.LogFormat($"日志内容:<color=#66ccff> --->   {message}   <--- </color>");
        }
        
        
        /// <summary>
        /// 坡道调整的代码，用于调整移动方向
        /// </summary>
        /// <param name="moveDirection"></param>
        /// <param name="originTransform"></param>
        /// <param name="detectionDistance"></param>
        /// <param name="groundLayer"></param>
        /// <returns></returns>
        public static Vector3 ModifyDirectionOnSlope(Vector3 moveDirection, Transform originTransform, float detectionDistance, LayerMask groundLayer)
        {

            /*
             * 当移动方向与坡道方向不一致时，一定会导致角色与坡道不贴合，如此导致角色悬空，这样会使重力影响角色速度，最终导致速度非常大。
             * 解决办法：当遇到斜坡时候，如果斜坡不为垂直方向，则将角色的移动方向投影到斜坡平面上，从而保证角色与斜坡贴合。
             */
            // 1. 首先检测是否有斜坡
            if (Physics.Raycast(originTransform.position + (originTransform.up * 0.5f) ,Vector3.down, out var hit, detectionDistance, groundLayer))
            {
                // 2. 检测是否为斜坡
                if ( Vector3.Dot(Vector3.up, hit.normal) != 0 )
                {
                    // 将方向投影到斜坡平面上
                    moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal);
                }
            }

            return moveDirection;
        }
    }
}
