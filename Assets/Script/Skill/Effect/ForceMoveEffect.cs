using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Effect
{

    public class EffectRuntimeForceMove:EffectRuntimeBase
    {
       public Vector3 direction;
       public float distance;
       internal override void Apply()
       {
           
       }
    }
    
    [LabelText("强制移动效果")]
    public class ForceMoveEffect:EffectBase
    {
        public Vector3 direction;
        public float distance;

        public override EffectMajorType MajorType { get; set; }
        public override EffectMinorType MinorType { get; set; }
        public override EffectRuntimeBase ConvertToRuntimeEffect(EntityBase caster, EntityBase target)
        {
            throw new System.NotImplementedException();
        }

    }
}