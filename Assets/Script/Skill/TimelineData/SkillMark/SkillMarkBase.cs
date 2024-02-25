using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic;
using Script.Skill.SkillLogic.SkillTimeline;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace Script.Skill.TimelineTrack
{
    [Serializable]
    public abstract class SkillMarkBase : Marker, ISerializationCallbackReceiver, IUseBlackBoardAsset
    {
        [LabelText("持续时间类型")]
        public SkillMarkPersistentTimeType PersistentTimeType;
        
        [LabelText("重播方式")]
        public SkillMarkReplyType SkillMarkReplyType;

        public IEnumerable<string> GetValue<T>()
        {
            var asset = (this as IUseBlackBoardAsset).GetTimeLine();
            return asset.GetBlackBoardKey<T>();
        }


        public virtual void OnBeforeSerialize()
        {
            (this as IUseBlackBoardAsset).SetContext();
        }

        public void OnAfterDeserialize()
        {
        }
    }
}