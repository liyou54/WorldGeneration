using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic.SkillTimeline;
using Script.Skill.TimelineTrack;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "伤害效果", menuName = "战斗/技能", order = 0)]
public abstract class SkillEntityTimeline : TimelineAsset, ISerializationCallbackReceiver
{
    [NonSerialized, ShowInInspector] public BlackBoardParamSet Data = new BlackBoardParamSet();
    [SerializeField, HideInInspector] private byte[] DataBytes;

    public void SetRequireBoardBlack()
    {
        Dictionary<string, (Type, object)> typeDict = new Dictionary<string, (Type, object)>()
        {
            { "TargetGo", (typeof(BlackBoardGameObjectParam), null) },
            { "TargetPos", (typeof(BlackBoardVector3Param), Vector3.zero) },
            { "CanBeInterrupt", (typeof(BlackBoardBoolParam), true) },
        };


        foreach (var requireKv in typeDict)
        {
            var key = requireKv.Key;
            var hasAdd = Data.Data.Any(obj => (obj as BlackBoardParam).Key == key);
            if (!hasAdd)
            {
                var (type,defaultValue) = requireKv.Value;
                var instance = Activator.CreateInstance(type);
                (instance as BlackBoardParam).Key = key;
                // 通过反射设置默认值
                var field = type.GetField("Value", BindingFlags.Public | BindingFlags.Instance);
                field.SetValue(instance, defaultValue);

                Data.Data.Add(instance);
            }
        }
    }

    public void OnBeforeSerialize()
    {
        SetRequireBoardBlack();

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
            Data.OnBeforeSerialize();
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


    public class UpdateBlackBoardInfo
    {
        public object TimelineObject;
        public IBlackBoardKey BlackBoardKeyObject;
    }

    public List<UpdateBlackBoardInfo> GetWillChangeBlackBoardKey(string oldKey)
    {
        var res = new List<UpdateBlackBoardInfo>();
        foreach (var track in GetOutputTracks())
        {
            if (track is SkillTrack skillTrack)
            {
                foreach (var clip in skillTrack.GetClips())
                {
                    var useBlackBoard = clip.asset as IUseBlackBoardAsset;
                    if (useBlackBoard != null)
                    {
                        var keys = useBlackBoard.GetTimelineBoardParamUsed();
                        foreach (var key in keys)
                        {
                            var isSetter = key.FieldType.IsGenericType && key.FieldType.GetGenericTypeDefinition() == typeof(SkillTimelineParamSetterBase<>);
                            var isGetter = key.FieldType.IsGenericType && key.FieldType.GetGenericTypeDefinition() == typeof(SkillTimelineParamGetterBase<>);
                            var value = key.GetValue(useBlackBoard);

                            if ((isSetter || isGetter) && (value as IBlackBoardKey).BlackBoardKeyField == oldKey)
                            {
                                res.Add(new UpdateBlackBoardInfo()
                                {
                                    TimelineObject = useBlackBoard,
                                    BlackBoardKeyObject = value as IBlackBoardKey
                                });
                            }
                        }
                    }
                }
            }
        }

        return res;
    }
}