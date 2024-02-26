using System;
using System.Collections.Generic;
using Battle.Effect;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


namespace Battle.Bullet
{
    [LabelText("子弹配置")]
    public abstract class BulletSO : ScriptableObject, ISerializationCallbackReceiver
    {
        [AssetsOnly] [LabelText("子弹预制体")] public BulletEntity BulletPrefab;
        public EffectListSerializeData EffectListSerializeData = new EffectListSerializeData();


        public void OnBeforeSerialize()
        {
            EffectListSerializeData.OnBeforeEffectListSerialize();
        }

        public void OnAfterDeserialize()
        {
            EffectListSerializeData.OnAfterEffectListDeserialize();
        }
    }
}