using System.Diagnostics;
using Script.Skill.BlackBoardParam;
using Debug = UnityEngine.Debug;

namespace Script.Skill.SkillLogic
{
    // 这个类用于控制循环
    public class SkillClipLoopLogic:SkillClipLogicBase,ISkillTimeJumpAble
    {
        public SkillTimelineParamGetterBase<bool> BreakCondition;
        public bool IsContinuePlayWhenTrue;
        
        
        
        public  void JumpSkillTime(SkillContext context)
        {
            var timelineTime = context.GetTimelineTime();
            var isBreak = BreakCondition.GetValue(context.SkillDataRuntime.BlackBoard);
            Debug.Log(timelineTime);
            var jumpToTimelineTime = -1f;
            var isOutTime = timelineTime >= EndTime;
            if (isBreak && !IsContinuePlayWhenTrue)
            {
                jumpToTimelineTime = EndTime ;
            }
            if (isOutTime && !isBreak)
            {
                jumpToTimelineTime = StartTime;
            }

            if (jumpToTimelineTime < 0)
            {
                return;
            }
            
            var lastLogic = context.SkillDataRuntime.GetLastLogicList();
            foreach (var logic in lastLogic)
            {
                if (jumpToTimelineTime >= logic.EndTime)
                {
                    logic.Status = SkillClipStatus.Finish;
                }

                if (jumpToTimelineTime < logic.StartTime)
                {
                    logic.Status = SkillClipStatus.NoStart;
                }
            }
            
            
            
            Debug.Log("跳转");
            context.LastTimeLineTime = jumpToTimelineTime;
            context.LoopStartTimelineTime = jumpToTimelineTime;
            context.LastLoopTime = context.AllRunTime;
            

        }


    }


}