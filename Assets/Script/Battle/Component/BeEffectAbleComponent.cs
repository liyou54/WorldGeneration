using System.Collections.Generic;
using Battle.Buffer;
using Script.EntityManager;
using UnityEngine;

namespace Battle.Effect
{
    public class BeEffectAbleComponent:EntityComponentBase,IUpdateAble
    {
        public List<BufferRuntimeBase> BufferList = new List<BufferRuntimeBase>();

        
        public void ApplyEffect(RuntimeEffectBase effect)
        {
            effect.Apply();
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

        }
    }
}