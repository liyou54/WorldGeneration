using Sirenix.OdinInspector;

namespace Battle.Effect
{
    
    public class RuntimeCauseDamageEffect:RuntimeEffectBase
    {
        public int Damage;
        public override void Apply()
        {
            
        }
    }
    

    [LabelText("伤害效果")]
    public class CauseDamageEffect:EffectBase
    {
        public int Damage;
        public override RuntimeEffectBase ConvertToRuntimeEffect()
        {
            var rt = new RuntimeCauseDamageEffect();
            rt.Damage = Damage;
            return rt;
        }
    }


}