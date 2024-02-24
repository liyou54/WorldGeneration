using System.ComponentModel;
using Script.Skill.BlackBoardParam;
using Script.Skill.RuntimeSkillData.SkillView;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;

namespace Script.Skill.TimelineTrack
{
    public enum SkillItemFollowType
    {
        None,
        Target,
        Character,
        Position,
        CustomTarget,
    }


    [DisplayName("技能/Mark/特效")]
    public class SkillFxMark : SkillMarkBase,ISkillMarkConvertToView
    {
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
        
        public SkillMarkViewBase Convert()
        {
            var res = new SkillMarkFxView(); 
            res.PatricleFx = PatricleFx;
            res.StartTime = (float)time;
            res.FollowType = FollowType;
            res.IsWorld = IsWorld;
            res.Offset = Offset;
            res.IsFollow = IsFollow;
            return res;
        }
    }


}