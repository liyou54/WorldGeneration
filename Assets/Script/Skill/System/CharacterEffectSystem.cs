using System.Collections.Generic;
using Battle.Buffer;
using Battle.Effect;
using Battle.Operation;
using Script.EntityManager;
using SGoap;

namespace Battle.Effect
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