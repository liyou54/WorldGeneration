using UnityEngine;

namespace Battle.Bullet
{
    [CreateAssetMenu(fileName = "子弹", menuName = "战斗/子弹", order = 0)]
    public class BulletSO : ScriptableObject
    {
        public GameObject BulletPrefab;
    }
}