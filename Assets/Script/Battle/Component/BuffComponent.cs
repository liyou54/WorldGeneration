using System.Collections.Generic;
using Script.Skill.Buff;
using Script.Skill.Effect;
using Script.Entity;
using Script.Skill;
using UnityEngine;

namespace Battle
{
    public class BuffComponent : EntityComponentBase, IAttachToSystem
    {
        public List<BuffRuntimeBase> BufferList = new List<BuffRuntimeBase>();
        private BuffTriggerSystem _buffTriggerSystem;

        public Dictionary<(ETargetEffect, EDecoratorTimePointType), List<BuffRuntimeBase>> EffectBuffListData = new()
        {
            { (ETargetEffect.Caster, EDecoratorTimePointType.Before), new() },
            { (ETargetEffect.Caster, EDecoratorTimePointType.After), new() },
            { (ETargetEffect.Target, EDecoratorTimePointType.Before), new() },
            { (ETargetEffect.Target, EDecoratorTimePointType.After), new() },
        };


        public void AddBuff(BuffRuntimeBase buff)
        {
            _buffTriggerSystem.AddBuff(buff);
        }

        internal void AddBuff_Internal(BuffRuntimeBase buff)
        {
            if (buff is EffectDecoratorBuffRuntime effectDecoratorBuffRuntime)
            {
                AddBuffToEffectList(effectDecoratorBuffRuntime);
            }
        }

        public List<BuffRuntimeBase> TryGetEffectDecoratorBuffList(EffectRuntimeBase effect,ETargetEffect targetType, EDecoratorTimePointType timePointType)
        {
            var key = (targetType, timePointType);
            EffectBuffListData.TryGetValue(key, out var tempRes);
            var res = new List<BuffRuntimeBase>();
            if (tempRes != null)
            {
                foreach (var buff in tempRes)
                {
                    if (buff is EffectDecoratorBuffRuntime effectDecoratorBuffRuntime && effectDecoratorBuffRuntime.IsMarch(effect))
                    {
                        res.Add(buff);
                    }
                }
            }
            return res;
        }


        private void AddBuffToEffectList(EffectDecoratorBuffRuntime buff)
        {
            var key = (buff.FlitterFrom, buff.TimerType);
            EffectBuffListData[key].Add(buff);
        }

        public override void Start()
        {
            var entityManager = EntityManager.Instance;
            _buffTriggerSystem = entityManager.TryGetOrAddSystem<BuffTriggerSystem>();
            _buffTriggerSystem.AddToUpdate(this);
        }


        public EAttachToSystemRunStatus RunStatus { get; set; }
    }
}