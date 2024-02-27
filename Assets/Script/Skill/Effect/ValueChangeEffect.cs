using Sirenix.OdinInspector;

namespace Battle.Effect
{
    
    public class RuntimeValueChangeEffect:RuntimeEffectBase
    {
        public int Value;
        public EffectKeyTable Key;
        public override void Apply()
        {
            
        }
    }
    
    [LabelText("数值变化效果")]
    public class ValueChangeEffect : EffectBase
    {
        [LabelText("效果值")] public int Value;
        [LabelText("效果类型")] public EffectKeyTable Key;
        public override RuntimeEffectBase ConvertToRuntimeEffect()
        {
            var rt = new RuntimeValueChangeEffect();
            rt.Value = Value;
            rt.Key = Key;
            return rt;
        }
    }
}