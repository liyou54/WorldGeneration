using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Bullet
{
    [CreateAssetMenu(fileName = "子弹", menuName = "战斗/子弹/飞行子弹", order = 0)]
    [LabelText("子弹配置")]
    public class AOEBulletSO : BulletSO
    {
        [LabelText("子弹飞行速度")] public float Speed;

    }
}