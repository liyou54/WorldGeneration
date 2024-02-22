using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Skill.BlackBoardParam;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Script.Skill.TimelineTrack
{
    
    [Serializable]
    public abstract class SkillClipBase : PlayableAsset, ITimelineClipAsset,ISerializationCallbackReceiver
    {

        public SkillEntityTimeline GetTimeLine()
        {
            var path = AssetDatabase.GetAssetPath(this);
            var asset = AssetDatabase.LoadAssetAtPath<SkillEntityTimeline>(path);
            return asset;
        }

        public IEnumerable<string> GetValue<T>()
        {
            var asset = GetTimeLine();
            return asset.GetBlackBoardKey<T>();
        }


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<EmptyBehavior>.Create(graph);
            return playable;
        }

        public ClipCaps clipCaps { get; }

        public void OnBeforeSerialize()
        {
            // 找到所有派生
            var type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in fields)
            {
                Type fieldType = field.FieldType;
                if (fieldType.IsGenericType &&
                    (fieldType.GetGenericTypeDefinition() == typeof(SkillTimelineParamGetterBase<>) ||
                     fieldType.GetGenericTypeDefinition() == typeof(SkillTimelineParamSetterBase<>))
                    )
                {
                    var value = field.GetValue(this);

                    if (value != null)
                    {
                        Func<SkillEntityTimeline> getTimeLineFunc = () => this.GetTimeLine();
                        FieldInfo timelineField = fieldType.GetField("Timeline", BindingFlags.Instance | BindingFlags.Public);
                        timelineField.SetValue(value, getTimeLineFunc);
                    }
                }
            }
        
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}