using System.Collections.Generic;
using Battle.Buffer;
using Script.EntityManager;
using UnityEngine;

namespace Battle.Effect
{
    public class BeEffectAbleComponent:EntityComponentBase,IUpdateAble
    {
        public List<BufferRuntimeBase> BufferList = new List<BufferRuntimeBase>();
        public Queue<RuntimeEffectBase> EffectList = new Queue<RuntimeEffectBase>();

        
        public void AddEffect(RuntimeEffectBase effect)
        {
            EffectList.Enqueue(effect);
        }
        
        public void AddEffectList(List<RuntimeEffectBase> effectList)
        {
            foreach (var effect in effectList)
            {
                EffectList.Enqueue(effect);
            }
        }
        
        public override void OnCreate()
        {
            
        }

        public override void OnDestroy()
        {
        }

        public override void Start()
        {
        }

        public void Update()
        {
            while (EffectList.Count > 0)
            {
                var effect = EffectList.Dequeue();
                effect.Apply();
            }
        }
    }
}