using UnityEngine;

namespace Battle.Effect
{
    [CreateAssetMenu(fileName = "伤害效果", menuName = "战斗/效果", order = 0)]

    public class DamageEffectData:EffectDataBase
    {
        public int Damage;
        public int DamageType;
    }
    
    
}