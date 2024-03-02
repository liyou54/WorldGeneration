using System;
using System.Collections.Generic;
using Script.Skill.Effect;
using Script.Entity;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Script.Skill.Buff
{
    [CreateAssetMenu(fileName = "空buff", menuName = "战斗/Buffer/空buff", order = 9999999)]
    public abstract class BuffSO : ScriptableObject,ISerializationCallbackReceiver,IConvertToRuntimeBuff
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

        public abstract BuffRuntimeBase ConvertToRuntimeBuff(EntityBase caster, EntityBase owner);
    }
}