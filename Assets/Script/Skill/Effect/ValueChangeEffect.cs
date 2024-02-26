using Sirenix.OdinInspector;

namespace Battle.Effect
{
    [LabelText("数值变化效果")]
    public class ValueChangeEffect : EffectBase
    {
        [LabelText("效果值")] public int Value;
        [LabelText("效果类型")] public EffectKeyTable Key;
    }
}