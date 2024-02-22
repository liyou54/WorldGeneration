using System;
using System.Collections.Generic;
using Animancer;
using JetBrains.Annotations;
using Script.Skill.BlackBoardParam;
using UnityEngine;

namespace Script.Skill.SkillLogic
{
    public class SkillContext
    {
        public float LastLoopTime; // 上一次循环的时间
        public float AllRunTime; // 总共运行时间
        public float LoopStartTimelineTime; // 循环开始Timeline时间
        public float DeltaTime; // 时间间隔
        public float LastTimeLineTime; // 上一次Timeline时间
        
        public GameObject Owner { get; set; }
        public SkillDataRuntime SkillDataRuntime { get; set; }
        public AnimancerComponent Animancer { get; set; }

        public bool TryGetBlackData<T> (string key, [NotNull] ref T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (SkillDataRuntime.BlackBoard.TryGetValue<T>(key, out var obj))
            {
                value = (T) obj;
                return true;
            }

            return false;
        }
        
        public void SetBlackData<T>(string key, T value)
        {
            SkillDataRuntime.BlackBoard.SetValue(key, value);
        }
        
        public float GetTimelineTime()
        {
            return AllRunTime - LastLoopTime + LoopStartTimelineTime;
        }
        
    }
}