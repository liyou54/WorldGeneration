using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Script.Skill.Effect
{
    [HideReferenceObjectPicker]
    [HideLabel]
    [Serializable]
    public class EffectListSerializeData
    {
        [LabelText("效果")] [ShowInInspector] [NonSerialized] public List<EffectBase> EffectList;
        [HideInInspector,SerializeField] private byte[] effectListBytes;

        public void OnBeforeEffectListSerialize()
        {
            if (EffectList != null)
            {
                effectListBytes = SerializationUtility.SerializeValue(EffectList, DataFormat.JSON);
            }
        }

        public void OnAfterEffectListDeserialize()
        {
            if (effectListBytes != null)
            {
                EffectList = SerializationUtility.DeserializeValue<List<EffectBase>>(effectListBytes, DataFormat.JSON);
            }
        }
    }
}