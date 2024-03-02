using System;
using System.Collections.Generic;
using Battle;
using Script.Skill.Buff;
using Script.Skill.Bullet;
using Script.Skill.Effect;
using Script.Entity;
using UnityEngine;

namespace Script.Skill
{
    public class BuffTriggerSystem : SystemBaseWithUpdateItem<BuffComponent>
    {
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
            var res = new List<BuffRuntimeBase>();

            if (effect.EffectTarget != null)
            {
                var targetEffect = effect.EffectTarget.GetEntityComponent<BuffComponent>();
                if (targetEffect != null)
                {
                    res.AddRange(targetEffect.TryGetEffectDecoratorBuffList(effect,ETargetEffect.Target, timePointType));
                }
            }
            if (effect.EffectCaster != null)
            {
                var casterEffect = effect.EffectCaster.GetEntityComponent<BuffComponent>();
                if (casterEffect != null)
                {
                    res.AddRange(casterEffect.TryGetEffectDecoratorBuffList(effect,ETargetEffect.Caster, timePointType));
                }
            }
            return res;
        }

        public void AddBuff(BuffRuntimeBase buff)
        {
            OnBuffAddBeforeDecorator(buff);
            if (buff != null && buff.Owner != null)
            {
                var buffComp = buff.Owner.GetEntityComponent<BuffComponent>();
                if (buffComp != null)
                {
                    buffComp.AddBuff_Internal(buff);
                }
            }

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


        public override void OnCreate()
        {
            // 注册效果 hook
            var entityManager = EntityManager.Instance;
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