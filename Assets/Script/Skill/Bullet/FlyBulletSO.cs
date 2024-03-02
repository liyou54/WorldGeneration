using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.Bullet
{
    [CreateAssetMenu(fileName = "子弹", menuName = "战斗/子弹/飞行子弹", order = 0)]
    [LabelText("子弹配置")]
    public class FlyBulletSO : BulletSO
    {
        [LabelText("子弹飞行速度")] public float Speed;
        [LabelText("子弹最远距离")] public float MaxDistance;
        [LabelText("子弹是否穿透")] public bool IsPenetrate;
        [LabelText("子弹是否追踪")] public bool IsTrack;
        
    }
}