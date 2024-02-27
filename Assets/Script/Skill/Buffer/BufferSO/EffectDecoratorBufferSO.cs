using Battle.Effect;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Buffer
{
    
    public class EffectDecoratorBufferRuntime : BufferRuntimeBase
    {
        public EDecoratorTimePointType TimerType;
        public EffectMajorType MajorTypeFilter;
        public EffectMinorType MinorTypeFilter;
        public ETargetEffect FlitterFrom;
        
        
        public EffectDecoratorBufferRuntime(EffectDecoratorBufferSO effectDecoratorBufferSo)
        {
        }
    }
    
    [CreateAssetMenu(fileName = "效果产生装饰器", menuName = "战斗/Buffer/效果产生装饰器")]
    public class EffectDecoratorBufferSO : BufferSO,IConvertToRuntimeBuffer
    {
        public EDecoratorTimePointType TimerType;
        [LabelText("激发大类效果过滤器")]  [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        public EffectMajorType MajorTypeFilter;

        [LabelText("激发细分效果过滤器")]  [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        public EffectMinorType MinorTypeFilter;

        [LabelText("来源过滤器")] [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        public ETargetEffect FlitterFrom;


        public BufferRuntimeBase ConvertToRuntimeBuffer(EntityBase caster, EntityBase target)
        {
            var res = new EffectDecoratorBufferRuntime(this);
            res.Caster = caster;
            res.Target = target;
            res.Name = Name;
            res.Priority = Priority;
            res.TimerType = TimerType;
            res.MajorTypeFilter = MajorTypeFilter;
            res.MinorTypeFilter = MinorTypeFilter;
            res.FlitterFrom = FlitterFrom;
            return res;
        }

    }


}