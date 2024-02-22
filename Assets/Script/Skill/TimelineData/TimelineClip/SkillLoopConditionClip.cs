using System.ComponentModel;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic;
using UnityEngine.Serialization;

namespace Script.Skill.TimelineTrack
{
    [DisplayName("技能/Clip/循环条件")]
    public class SkillLoopConditionClip : SkillClipBase, IClipConvertToLogic
    {
        public SkillTimelineParamGetterBase<bool> BreakCondition;
        public bool IsPlayContinue;

        public SkillClipLogicBase Convert(float start, float end)
        {
            var logic = new SkillClipLoopLogic();
            logic.BreakCondition = BreakCondition;
            logic.StartTime = start;
            logic.EndTime = end;
            logic.IsPlayContinue = IsPlayContinue;
            return logic;
        }
    }
}