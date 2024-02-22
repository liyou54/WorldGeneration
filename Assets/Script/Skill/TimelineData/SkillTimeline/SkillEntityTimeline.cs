using System;
using System.Collections.Generic;
using System.Linq;
using Script.Skill.BlackBoardParam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "伤害效果", menuName = "战斗/技能", order = 0)]
public abstract class SkillEntityTimeline : TimelineAsset, ISerializationCallbackReceiver
{
    [NonSerialized, ShowInInspector ] public BlackBoardParamSet Data = new BlackBoardParamSet();
    [SerializeField, HideInInspector] private byte[] DataBytes;

    public void OnBeforeSerialize()
    {
        if (Data != null)
        {
            DataBytes = SerializationUtility.SerializeValue(Data, DataFormat.JSON);
        }
    }

    public void OnAfterDeserialize()
    {
        if (DataBytes != null)
        {
            Data = SerializationUtility.DeserializeValue<BlackBoardParamSet>(DataBytes, DataFormat.JSON);
        }
    }

    public IEnumerable<string> GetBlackBoardKey<T>()
    {
        return Data.GetBlackBoardKeyByType<T>();
    }



    public IEnumerable<string> GetGroupNames()
    {
        return GetOutputTracks().Where(((asset, i) => asset.parent is GroupTrack)).Select(track => track.parent.name).Distinct();
    }
    
}