using System.ComponentModel;
using Script.Skill.BlackBoardParam;
using Script.Skill.SkillLogic;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace Script.Skill.TimelineTrack
{
    [DisplayName("技能/Clip/循环条件")]
    public class SkillLoopConditionClip : SkillClipBase, IClipConvertToLogic
    {
        [LabelText("循环条件")]
        public SkillTimelineParamGetterBase<bool> BreakCondition;
        [LabelText("达到条件是否继续播放")]
        public bool IsContinuePlayWhenTrue;

        public SkillClipLogicBase Convert(float start, float end)
        {
            var logic = new SkillClipLoopLogic();
            logic.BreakCondition = BreakCondition;
            logic.StartTime = start;
            logic.EndTime = end;
            logic.IsContinuePlayWhenTrue = IsContinuePlayWhenTrue;
            return logic;
        }
    }
}