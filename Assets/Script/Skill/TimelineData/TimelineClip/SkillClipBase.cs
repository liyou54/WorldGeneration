using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic.SkillTimeline;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Script.Skill.TimelineTrack
{
    [Serializable]
    public abstract class SkillClipBase : PlayableAsset, ITimelineClipAsset, IUseBlackBoardAsset, ISerializationCallbackReceiver
    {
        public IEnumerable<string> GetValue<T>()
        {
            var asset = (this as IUseBlackBoardAsset).GetTimeLine();
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
            (this as IUseBlackBoardAsset).SetContext();
        }

        public void OnAfterDeserialize()
        {
        }
    }
}