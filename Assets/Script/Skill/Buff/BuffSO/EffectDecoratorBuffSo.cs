using System.Collections.Generic;
using Script.Skill.Effect;
using Script.Entity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Skill.Buff
{
    public class EffectDecoratorBuffRuntime : BuffRuntimeBase
    {
        public EDecoratorTimePointType TimerType;
        public EffectMajorType MajorTypeFilter;
        public EffectMinorType MinorTypeFilter;
        public ETargetEffect FlitterFrom;
        public List<EffectBase> EffectList;

        public EffectDecoratorBuffRuntime(EffectDecoratorBuffSo effectDecoratorBuffSo,EntityBase caster,EntityBase owner)
        {
            EffectList = effectDecoratorBuffSo.effectListSerializeData.EffectList;
            Name = effectDecoratorBuffSo.Name;
            Priority = effectDecoratorBuffSo.Priority;
            TimerType = effectDecoratorBuffSo.TimerType;
            MajorTypeFilter = effectDecoratorBuffSo.MajorTypeFilter;
            MinorTypeFilter = effectDecoratorBuffSo.MinorTypeFilter;
            FlitterFrom = effectDecoratorBuffSo.FlitterFrom;
            Caster = caster;
            Owner = owner;
        }

        public bool IsMarch(EffectRuntimeBase effectRuntimeBase)
        {
            if (MinorTypeFilter != EffectMinorType.EffectMinorSetting.None && effectRuntimeBase.MinorType != MinorTypeFilter)
            {
                return false;
            }
            
            if (MajorTypeFilter != EffectMajorType.EffectMajorSetting.None
                && effectRuntimeBase.MajorType != MajorTypeFilter 
                && MinorTypeFilter == EffectMinorType.EffectMinorSetting.None)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="trigger"> buffer触发者 </param>
        internal override void Execute(EntityBase trigger)
        {
            var entityManager = EntityManager.Instance;
             var effectSystem = entityManager.TryGetOrAddSystem<CharacterEffectSystem>();
            foreach (var effect in EffectList)
            {
                if (effect.TargetEffect == ETargetEffect.Target)
                {
                    var runtimeEffect = effect.ConvertToRuntimeEffect(trigger, Owner);
                    effectSystem.ExecuteEffect(runtimeEffect);
                }
                if (effect.TargetEffect == ETargetEffect.Caster)
                {
                    var runtimeEffect =  effect.ConvertToRuntimeEffect(Owner, trigger);
                    effectSystem.ExecuteEffect(runtimeEffect);
                }
            }
        }
    }

    [CreateAssetMenu(fileName = "效果产生装饰器", menuName = "战斗/Buffer/效果产生装饰器")]
    public class EffectDecoratorBuffSo : BuffSO
    {
        public EDecoratorTimePointType TimerType;

        [LabelText("激发大类效果过滤器")] [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        public EffectMajorType MajorTypeFilter;

        [LabelText("激发细分效果过滤器")] [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        public EffectMinorType MinorTypeFilter;

        [LabelText("来源过滤器")] [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
        public ETargetEffect FlitterFrom;


        /// <summary>
        /// </summary>
        /// <param name="caster">施加者</param>
        /// <param name="target">施加目标</param>
        /// <returns></returns>
        public override BuffRuntimeBase ConvertToRuntimeBuff(EntityBase caster, EntityBase target)
        {
            var res = new EffectDecoratorBuffRuntime(this,caster,target);
            return res;
        }
    }
}