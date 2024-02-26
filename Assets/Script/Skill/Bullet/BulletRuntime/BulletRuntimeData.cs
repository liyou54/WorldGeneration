using UnityEngine;

namespace Battle.Bullet.BulletRuntime
{
    public class BulletRuntimeData
    {
        public GameObject TargetGo { get; set; }
        public Vector3 TargetPos { get; set; }
        public BulletSO BulletSo { get; set; }
    }
}