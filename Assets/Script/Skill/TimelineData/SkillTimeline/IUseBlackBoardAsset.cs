using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Skill.BlackBoardParam;
using UnityEditor;
using UnityEngine;

namespace Script.Skill.SkillLogic.SkillTimeline
{
    public interface IUseBlackBoardAsset
    {

        public SkillEntityTimeline GetTimeLine()
        {
            var path = AssetDatabase.GetAssetPath(this as UnityEngine.Object);
            var asset = AssetDatabase.LoadAssetAtPath<SkillEntityTimeline>(path);
            return asset;
        }
        
        public List<FieldInfo> GetTimelineBoardParamUsed()
        {
            var res = new List<FieldInfo>();
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
                    res.Add(field);
                }
            }

            return res;
        }

        public void SetContext()
        {
            
            var type = this.GetType();
            var fields = (this as IUseBlackBoardAsset).GetTimelineBoardParamUsed();
            foreach (var field in fields)
            {
                Type fieldType = field.FieldType;
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

}