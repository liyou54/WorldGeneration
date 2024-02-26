using System;
using System.Collections.Generic;
using Battle.Effect;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Battle.Buffer
{
    [CreateAssetMenu(fileName = "空buff", menuName = "战斗/Buffer", order = 0)]
    public  class BufferSO : ScriptableObject
    {
        [LabelText("Buff名称")] public readonly String Name;
        [LabelText("优先级")] public readonly int Priority;
        [LabelText("持续时间")] public readonly float Duration;
        public readonly EBufferEffectTime EffectTime;
        [LabelText("效果列表")] public readonly List<EffectBase> EffectList = new List<EffectBase>();
    }
}