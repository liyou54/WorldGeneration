using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Effect
{
    
    public class RuntimeCauseDamageEffect:RuntimeEffectBase
    {
        public int Damage;
        public override void Apply()
        {
            Debug.Log("造成" + Damage + "点伤害");
        }
    }
    


    [LabelText("普通伤害效果")]
    public class CauseDamageEffect:EffectBase
    {
        public int Damage;


        [field:SerializeField]  public override EffectMajorType MajorType { get; set; }
        [field:SerializeField]  public override EffectMinorType MinorType { get; set; }

        public override RuntimeEffectBase ConvertToRuntimeEffect()
        {
            var rt = new RuntimeCauseDamageEffect();
            rt.Damage = Damage;
            return rt;
        }

    }

    
    


}