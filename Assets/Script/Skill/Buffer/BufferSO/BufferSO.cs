using System;
using System.Collections.Generic;
using Battle.Effect;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Battle.Buffer
{
    [CreateAssetMenu(fileName = "空buff", menuName = "战斗/Buffer/空buff", order = 9999999)]
    public  class BufferSO : ScriptableObject,ISerializationCallbackReceiver
    {
        [LabelText("Buff名称")] public String Name;
        [LabelText("优先级")] public int Priority;

        public EffectListSerializeData effectListSerializeData = new EffectListSerializeData();
        
        public void OnBeforeSerialize()
        {
            effectListSerializeData.OnBeforeEffectListSerialize();
        }

        public void OnAfterDeserialize()
        {
            effectListSerializeData.OnAfterEffectListDeserialize();
        }
    }
}