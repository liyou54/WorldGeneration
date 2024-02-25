using System;
using System.ComponentModel;
using Animancer;
using Script.Skill.RuntimeSkillData.SkillView;
using Script.Skill.SkillLogic;
using UnityEngine;

namespace Script.Skill.TimelineTrack
{
    
    [DisplayName("技能/Clip/动作")]
    public class SkillAnimationClip:SkillClipBase,IClipConvertToView
    {
        public AnimationClip AnimationClip;


        public SkillClipExecute ConvertToView(float start, float end)
        {
            var view = new SkillClipClipAnimationView();
            view.AnimationClip = AnimationClip;
            view.StartTime = start;
            view.EndTime = end;
            return view;
        }

    }
}