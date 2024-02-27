using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Effect
{

    public class RuntimeForceMoveEffect:RuntimeEffectBase
    {
       public Vector3 direction;
       public float distance;
       public override void Apply()
       {
           
       }
    }
    
    [LabelText("强制移动效果")]
    public class ForceMoveEffect:EffectBase
    {
        public Vector3 direction;
        public float distance;
        public override RuntimeEffectBase ConvertToRuntimeEffect()
        {
            var rt = new RuntimeForceMoveEffect();
            rt.direction = direction;
            rt.distance = distance;
            return rt;
        }
    }
}