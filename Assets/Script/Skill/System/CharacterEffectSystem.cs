using System.Collections.Generic;
using Script.Skill.Buff;
using Script.Skill.Effect;
using Battle.Operation;
using Script.Entity;

namespace Script.Skill.Effect
{
    public delegate EffectRuntimeBase EffectDecorator(EffectRuntimeBase effect);

    public class CharacterEffectSystem : SystemBase
    {
        public event EffectDecorator OnEffectBeforeDecorator;
        public event EffectDecorator OnEffectAfterDecorator;

        public void ExecuteEffect(EffectRuntimeBase effect)
        {
            OnEffectBeforeDecorator?.Invoke(effect);
            effect.Apply();
            OnEffectAfterDecorator?.Invoke(effect);
        }

        public override void Update(float deltaTime)
        {
        }
    }
}