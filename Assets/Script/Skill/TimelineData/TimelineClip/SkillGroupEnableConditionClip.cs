using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;

namespace Script.Skill.TimelineTrack
{
    [DisplayName("技能/Clip/技能组开启条件")]
    public class SkillGroupEnableConditionClip : SkillClipBase
    {
        [ValueDropdown("GetTrackGroupName")]
        public string ConditionSpace;
        [ValueDropdown("GetTrackGroupName")]
        public string TrueGroup;
        [ValueDropdown("GetTrackGroupName")]
        public string FalseGroup;
        public bool ConditionValue;
        
        public IEnumerable<string> GetTrackGroupName()
        {
            var timeline =GetTimeLine();
            return timeline.GetGroupNames();
        }
 
    }
}