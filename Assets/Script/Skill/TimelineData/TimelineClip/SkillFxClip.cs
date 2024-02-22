using System;
using System.Collections.Generic;
using System.ComponentModel;
using Animancer;
using Script.Skill.BlackBoardParam;
using Script.Skill.RuntimeSkillData.SkillView;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Script.Skill.TimelineTrack
{
    [DisplayName("技能/Clip/特效")]
    public class SkillFxClip : SkillClipBase,IClipConvertToView
    {
        public TrailRenderer TrailRenderer;
        public ParticleSystem PatricleFx;
        
        [EnumToggleButtons] [LabelText("特效触发位置种类")]
        public SkillItemFollowType FollowType;

        [LabelText("是否是世界坐标")] public bool IsWorld;
        [LabelText("出现位置偏移")] public Vector3 Offset;
        [ShowIf("@this.FollowType== SkillItemFollowType.CustomTarget ||" +
                "this.FollowType == SkillItemFollowType.Target ||" +
                "this.FollowType == SkillItemFollowType.Character")]
        [LabelText("是否跟随目标")]
        public bool IsFollow;

        public SkillClipViewBase Convert(float start, float end)
        {
            var res = new SkillClipFxView();
            res.PatricleFx = PatricleFx;
            res.TrailRenderer = TrailRenderer;
            res.StartTime = start;
            res.EndTime = end;
            res.FollowType = FollowType;
            res.IsWorld = IsWorld;
            res.Offset = Offset;
            res.IsFollow = IsFollow;
            return res;
            
        }
    }
}