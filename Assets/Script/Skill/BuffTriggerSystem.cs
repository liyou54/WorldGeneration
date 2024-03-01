using System;
using System.Collections.Generic;
using Battle.Buffer;
using Battle.Bullet;
using Battle.Effect;
using Script.EntityManager;
using UnityEngine;

namespace Script.Skill
{
    public class BuffTriggerSystem : SystemBaseWithUpdateItem<BuffRuntimeBase>
    {
        // Buff 触发会从这里面筛选
        // 效果筛选列表 效果产生时间
        // buff对象 比如反伤Buff就是效果从效果接收者触发buff 吸血Buff就是效果从效果施加者触发buff
        Dictionary<(ETargetEffect, EDecoratorTimePointType), Dictionary<EntityBase, List<BuffRuntimeBase>>> checkLists = new()
        {
            { (ETargetEffect.Caster, EDecoratorTimePointType.Before), new() },
            { (ETargetEffect.Caster, EDecoratorTimePointType.After), new() },
            { (ETargetEffect.Target, EDecoratorTimePointType.Before), new() },
            { (ETargetEffect.Target, EDecoratorTimePointType.After), new() },
        };


        // 角色身上挂载的所有 Buff
        public Dictionary<EntityBase, List<BuffRuntimeBase>> EntityAllBuffList = new();

        public void OnBuffAddBeforeDecorator(BuffRuntimeBase addBuff)
        {
        }

        public void OnBuffAddAfterDecorator(BuffRuntimeBase addBuff)
        {
        }

        public void OnBuffRemoveBeforeDecorator(BuffRuntimeBase removeBuff)
        {
        }

        public void OnBuffRemoveAfterDecorator(BuffRuntimeBase removeBuff)
        {
        }

        public void OnBuffExecuteBeforeDecorator(BuffRuntimeBase buff)
        {
        }

        public void OnBuffExecuteAfterDecorator(BuffRuntimeBase buff)
        {
        }

        private EffectRuntimeBase OnEffectBeforeExecute(EffectRuntimeBase effect)
        {
            var buffList = TryEffectGetCurrentTriggerBuffList(effect, EDecoratorTimePointType.Before);
            foreach (var buff in buffList)
            {
                ExecuteBuff(buff, effect.EffectCaster);
            }
            return effect;
        }

        private EffectRuntimeBase OnEffectAfterExecute(EffectRuntimeBase effect)
        {
            var buffList = TryEffectGetCurrentTriggerBuffList(effect, EDecoratorTimePointType.After);
            foreach (var buff in buffList)
            {
                ExecuteBuff(buff, effect.EffectCaster);
            }
            return effect;
        }

        private List<BuffRuntimeBase> TryEffectGetCurrentTriggerBuffList(EffectRuntimeBase effect, EDecoratorTimePointType timePointType)
        {
            var keyTarget = (ETargetEffect.Target, timePointType);
            var keyCaster = (ETargetEffect.Caster, timePointType);

            var res = new List<BuffRuntimeBase>();

            if (effect.EffectTarget != null && checkLists[keyTarget].TryGetValue(effect.EffectTarget, out var targetList))
            {
                foreach (var buff in targetList)
                {
                    if (buff is EffectDecoratorBuffRuntime effectDecoratorBuffRuntime)
                    {
                        if (effectDecoratorBuffRuntime.IsMarch(effect))
                        {
                            res.Add(buff);
                        }
                    }
                }
            }

            if (effect.EffectCaster != null && checkLists[keyCaster].TryGetValue(effect.EffectCaster, out var casterList))
            {
                foreach (var buff in casterList)
                {
                    if (buff is EffectDecoratorBuffRuntime effectDecoratorBuffRuntime)
                    {
                        if (effectDecoratorBuffRuntime.IsMarch(effect))
                        {
                            res.Add(buff);
                        }
                    }
                }
            }
            return res;
        }

        public void AddBuff(BuffRuntimeBase buff)
        {
            OnBuffAddBeforeDecorator(buff);
            AddBuffToCheckList(buff);
            // TODO Add Buffer
            OnBuffAddAfterDecorator(buff);
        }


        public void RemoveBuff(BuffRuntimeBase buff)
        {
            OnBuffRemoveBeforeDecorator(buff);

            // TODO Remove Buffer
            OnBuffRemoveAfterDecorator(buff);
        }

        private void ExecuteBuff(BuffRuntimeBase buff, EntityBase trigger = null)
        {
            OnBuffExecuteBeforeDecorator(buff);
            buff.Execute(trigger);
            OnBuffExecuteAfterDecorator(buff);
        }


        private void AddBuffToCheckList(BuffRuntimeBase buff)
        {
            if (buff is EffectDecoratorBuffRuntime effectDecoratorBuffRuntime)
            {
                AddBuffToEffectCheckList(effectDecoratorBuffRuntime);
            }
        }

        private void AddBuffToEffectCheckList(EffectDecoratorBuffRuntime buff)
        {
            var key = (buff.FlitterFrom, buff.TimerType);
            if (checkLists[key].ContainsKey(buff.Owner))
            {
                checkLists[key][buff.Owner].Add(buff);
            }
            else
            {
                checkLists[key].Add(buff.Owner, new List<BuffRuntimeBase> { buff });
            }
        }


        public override void OnCreate()
        {
            // 注册效果 hook
            var entityManager = global::EntityManager.Instance;
            var effectSystem = entityManager.TryGetOrAddSystem<CharacterEffectSystem>();
            effectSystem.OnEffectBeforeDecorator += OnEffectBeforeExecute;
            effectSystem.OnEffectAfterDecorator += OnEffectAfterExecute;

            // 注册操作 hook
            var skillSystem = entityManager.TryGetOrAddSystem<SkillSystem>();
            skillSystem.OnSkillBeforeDecorator += OnSkillBeforeExecute;
            skillSystem.OnSkillAfterDecorator += OnSkillAfterExecute;
        }

        private void OnSkillBeforeExecute(ref SkillPlay skill)
        {
        }

        private void OnSkillAfterExecute(ref SkillPlay skill)
        {
        }
    }
}