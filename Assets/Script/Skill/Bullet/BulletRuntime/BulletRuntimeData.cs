using Battle.Effect;
using UnityEngine;

namespace Battle.Bullet.BulletRuntime
{
    public class BulletRuntimeData
    {
        public EntityBase Caster { get; set; }
        public EntityBase TargetGo { get; set; }
        public Vector3 TargetPos { get; set; }
        public BulletSO BulletSo { get; set; }
    }
}