using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace Script.Skill.TimelineTrack
{
    
    [Serializable]
    public abstract class SkillMarkBase : Marker,ISerializationCallbackReceiver
    {
        public SkillMarkReplyType SkillMarkReplyType;
        
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


        public void OnBeforeSerialize()
        {
            // 找到所有派生
            var type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in fields)
            {
                Type fieldType = field.FieldType;
                if (fieldType.IsGenericType &&
                    fieldType.GetGenericTypeDefinition() == typeof(SkillTimelineParamGetterBase<>))
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