using System;
using System.Collections.Generic;
using Battle.Effect;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Battle.Buffer
{
    [CreateAssetMenu(fileName = "Buffer", menuName = "战斗/Buffer", order = 0)]
    public class BufferData : ScriptableObject
    {
        public String Name;
        public int Priority;
        public float Duration;
        [ShowInInspector] public EBufferEffectTime EffectTime;
        [SerializeReference] public List<EffectDataBase> EffectList = new List<EffectDataBase>();
    }
}