using System.Collections.Generic;
using Battle.Buffer;
using Battle.Effect;
using Script.EntityManager;
using UnityEngine;

namespace Battle
{
    
    public class BeEffectAbleComponent:EntityComponentBase
    {
        public List<BuffRuntimeBase> BufferList = new List<BuffRuntimeBase>();
        
        public void ApplyEffect(EffectRuntimeBase effect)
        {
            var characterEffectSystem = EntityManager.Instance.TryGetOrAddSystem<CharacterEffectSystem>();
            characterEffectSystem.ExecuteEffect(effect);
        }
    }
}