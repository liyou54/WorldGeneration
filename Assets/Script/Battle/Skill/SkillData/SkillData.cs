using UnityEngine;
using UnityEngine.Serialization;

namespace Battle.Operation.SkillData
{
    public class SkillData:ScriptableObject
    {
        public ESkillTargetFunctionType  targetFunctionType;
        public EBulletType BulletType;
        public ESkillDurationType DurationType;
        
    }
}